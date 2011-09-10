using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for FileAnimationEditor.xaml
    /// </summary>
    public partial class FileAnimationEditor : UserControl
    {
        #region Variables
        XDocument doc;
        PngBitmapEncoder png;
        double DpiX;
        double DpiY;
        int pow;
        #endregion Variables

        public FileAnimationEditor()
        {
            this.InitializeComponent();

            SaveSpriteSheetButton.MouseUp += new MouseButtonEventHandler(SaveSpriteSheetButton_MouseUp);
            ShowSpriteSheetButton.MouseUp += new MouseButtonEventHandler(ShowSpriteSheetButton_MouseUp);
            CancelButton.MouseUp += new MouseButtonEventHandler(CancelButton_MouseUp);
            EditFrame.MouseUp += new MouseButtonEventHandler(EditFrame_MouseUp);
            Okay.MouseUp += new MouseButtonEventHandler(Okay_MouseUp);
        }



        #region EventHandlers
        void EditFrame_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SpriteFrames.SelectedIndex > -1)
            {
                Frame f = SpriteFrames.SelectedValue as Frame;
                if (f != null)
                {
                    Workspace.Children.Add(new EditFrame(f) { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center });
                }
            }
            EditFrame.Visibility = Visibility.Collapsed;
        }

        void Okay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EditFrame.Visibility = Visibility.Visible;
        }

        void ShowSpriteSheetButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SpriteFrames.DataContext != null)
            {
                ObservableCollection<Frame> frames = (SpriteFrames.DataContext as AnimFile).Frames;

                if (frames != null)
                {
                    //find maxheight and width
                    int maxHeight = 0;
                    int maxWidth = 0;
                    foreach (Frame frame in frames)
                    {
                        if (maxHeight < frame.Height)
                            maxHeight = frame.Height;
                        if (maxWidth < frame.Width)
                            maxWidth = frame.Width;
                    }

                    int horizTileCount = (int)Math.Ceiling(Math.Sqrt(frames.Count));

                    int max = maxHeight < maxWidth ? maxWidth : maxHeight;

                    //calculate the best box for the maxsize tiles (square powers of 2)
                    pow = 2;
                    while (pow < max * horizTileCount)
                    {
                        pow *= 2;
                    }

                    //create Xdoc to store data about frames
                    doc = new XDocument();
                    doc.Add(new XElement("SpriteSheet"));

                    //create an object to draw the images onto
                    //Sheet.MaxHeight = pow;
                    //Sheet.MaxWidth = pow;
                    Sheet.Children.Clear();
                    Sheet.ColumnDefinitions.Clear();
                    Sheet.RowDefinitions.Clear();
                    for (int i = 0; i < horizTileCount; i++)
                    {
                        Sheet.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                        Sheet.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    }

                    //add images to the grid
                    for (int i = 0, count = frames.Count; i < count; i++)
                    {
                        Frame f = frames[i];
                        Image img = new Image() { Source = f.Image };
                        int row = i / horizTileCount;
                        int col = i % horizTileCount;
                        Grid.SetRow(img, row);
                        Grid.SetColumn(img, col);
                        Sheet.Children.Add(img);
                        XElement elem = new XElement("Frame");
                        elem.SetAttributeValue("Height", f.Height);
                        elem.SetAttributeValue("Width", f.Width);
                        elem.SetAttributeValue("TLPos", new Point(col * f.Width, row * f.Height));
                        doc.Root.Add(elem);
                    }
                    DpiX = frames[0].Image.DpiX;
                    DpiY = frames[0].Image.DpiY;
                    ToggleSpriteSheetVisible();
                }
            }
        }

        void SaveSpriteSheetButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            UIElement element = Sheet as UIElement;

            //get sheet's render size
            Size size = element.RenderSize;

            //render grid into a bitmap
            RenderTargetBitmap rtb = new RenderTargetBitmap(pow, pow, DpiX, DpiY, PixelFormats.Pbgra32);
            element.Measure(size);
            element.Arrange(new Rect(size));
            rtb.Render(element);
            png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Png Files(*.png)|*.png";
            if (sfd.ShowDialog() == true)
            {
                using (Stream s = File.Create(sfd.FileName))
                {
                    png.Save(s);
                }

                doc.Save(string.Format("{0}.spsh", sfd.FileName));

                string str = sfd.FileName.Substring(sfd.FileName.LastIndexOf('\\')+1);
                (DataContext as AnimFile).Filename = str.Substring(0, str.Length - str.LastIndexOf('.'));
            }
            ToggleSpriteSheetVisible();
        }

        void CancelButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ToggleSpriteSheetVisible();
        }
        #endregion EventHandlers

        #region Methods
        private void ToggleSpriteSheetVisible()
        {
            ShowSpriteSheetButton.Visibility = ShowSpriteSheetButton.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        #endregion Methods
    }

    public class VisibilityInverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}