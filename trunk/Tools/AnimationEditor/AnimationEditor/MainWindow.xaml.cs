using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Win32;
using System.IO;
using System.Xml.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            storage.mainWindow = this;
        }

        public readonly static RoutedUICommand CommandCreateSpriteSheet;
        public readonly static RoutedUICommand CommandSaveAndExit;
        public readonly static RoutedUICommand CommandImportSpriteSheet;
        public readonly static RoutedUICommand CommandImportImage;

        static MainWindow()
        {
            CommandCreateSpriteSheet = new RoutedUICommand("CreateSpriteSheet",
                "Create Sprite Sheet", typeof(MainWindow));
            CommandCreateSpriteSheet.InputGestures.Add(
                new KeyGesture(Key.E, ModifierKeys.Control));

            CommandSaveAndExit = new RoutedUICommand("SaveAndExit",
                "Exit", typeof(MainWindow));
            CommandSaveAndExit.InputGestures.Add(
                new KeyGesture(Key.Q, ModifierKeys.Control));

            CommandImportSpriteSheet = new RoutedUICommand("ImportSpriteSheet",
                "Import Sprite Sheet", typeof(MainWindow));
            CommandImportSpriteSheet.InputGestures.Add(
                new KeyGesture(Key.E, ModifierKeys.Control));

            CommandImportImage = new RoutedUICommand("ImportImage",
                            "Import Sprite Sheet", typeof(MainWindow));
            CommandImportImage.InputGestures.Add(
                new KeyGesture(Key.E, ModifierKeys.Control));
        }

        #region MenuFunctions
        private void Yes(Object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CanSave(Object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Files.HasItems;
        }

        public void LoadFromAnimFile(Object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog loadAnimDialog = new OpenFileDialog();
            loadAnimDialog.InitialDirectory = @"./";
            loadAnimDialog.Title = "Select animation file to load.";
            loadAnimDialog.Multiselect = false;
            loadAnimDialog.Filter = "Animation files (*.anim)|*.anim|All files (*.*)|*.*";
            if (loadAnimDialog.ShowDialog() == true)
            {
                ParseAnimfile(loadAnimDialog.OpenFile());
            }
        }

        public void SaveFile(Object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SaveFileAs(Object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void CreateAnimation(Object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void CreateSpriteSheet(Object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SaveAndExit(Object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void ImportSpriteSheet(Object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog loadAnimDialog = new OpenFileDialog();
            loadAnimDialog.InitialDirectory = @"C:\Users\Ivan Lloyd\Dropbox\GameDesign\BodilyInfection\BodilyInfection\BodilyInfectionContent\Sprites";
            loadAnimDialog.Title = "Select image file to load.";
            loadAnimDialog.Multiselect = true;
            loadAnimDialog.Filter = "Image Files(*.spsh)|*.spsh|All files (*.*)|*.*";
            if (loadAnimDialog.ShowDialog() == true)
            {
                ParseSpriteSheet(loadAnimDialog.FileName);
            }
        }

        public void ImportImage(Object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog loadAnimDialog = new OpenFileDialog();
            loadAnimDialog.InitialDirectory = @"C:\Users\Ivan Lloyd\Dropbox\GameDesign\BodilyInfection\BodilyInfection\BodilyInfectionContent\Sprites";
            loadAnimDialog.Title = "Select image file(s) to load.";
            loadAnimDialog.Multiselect = true;
            loadAnimDialog.Filter = "Image Files(*.BMP;*PNG;*.JPG;*.GIF)|*.BMP;*.PNG;*.JPG;*.GIF|All files (*.*)|*.*";
            if (loadAnimDialog.ShowDialog() == true)
            {
                ObservableCollection<Frame> frames = new ObservableCollection<Frame>();
                string[] fileNames = loadAnimDialog.FileNames;
                int count = 0;
                foreach (var file in loadAnimDialog.OpenFiles())
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.StreamSource = file;
                    img.EndInit();
                    frames.Add(new Frame()
                    {
                        File = fileNames[count],
                        Image = img,
                        AnimationPeg = new Point(0, 0),
                        Height = img.PixelHeight,
                        Width = img.PixelWidth,
                        StartPos = new Point(0, 0),
                        Pause = 20,
                        ClearColor = Colors.Magenta
                    }
                    );
                    count++;
                }
                if (Files.HasItems && Files.Items.Count > 0)
                {
                    Files.Items.MoveCurrentToFirst();
                    (Files.SelectedItem as TabItem).DataContext = new AnimFile(frames);
                }
                else
                {
                    Files.Items.Clear();
                    FileAnimationEditor fae = new FileAnimationEditor();
                    fae.DataContext = new AnimFile(frames);
                    Files.ItemsSource = new List<FileAnimationEditor>() { fae };
                    Files.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region Methods

        private void ParseAnimfile(Stream stream)
        {
            throw new NotImplementedException();
        }

        private void ParseSpriteSheet(string FileName)
        {
            List<Frame> frames = new List<Frame>();
            string spritesheetfile = FileName.Remove(FileName.LastIndexOf("."));
            int count = 0;

            BitmapImage spritesheet = new BitmapImage();
            spritesheet.BeginInit();
            spritesheet.StreamSource = new StreamReader(spritesheetfile).BaseStream;
            spritesheet.EndInit();

            XDocument doc = XDocument.Load(FileName);
            foreach (var frame in doc.Descendants("Frame"))
            {
                int h = int.Parse(frame.Attribute("Height").Value);
                int w = int.Parse(frame.Attribute("Width").Value);
                string[] spos = frame.Attribute("TLPos").Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Point TL = new Point(int.Parse(spos[0]), int.Parse(spos[1]));
                CroppedBitmap img = new CroppedBitmap(spritesheet, new Int32Rect((int)TL.X, (int)TL.Y, w, h));

                frames.Add(new Frame()
                {
                    File = spritesheetfile,
                    Image = img,
                    AnimationPeg = new Point(0, 0),
                    Height = h,
                    Width = w,
                    StartPos = TL,
                    Pause = 20,
                    ClearColor = Colors.Magenta
                }
                );
                count++;
            }
            //if (Files.HasItems && Files.Items.Count > 0)
            //{
            //    Files.Items.MoveCurrentToFirst();
            //    (Files.SelectedItem as TabItem).DataContext = new AnimFile(frames);
            //}
            //else
            //{
            //    Files.Items.Clear();
            //    FileAnimationEditor fae = new FileAnimationEditor();
            //    fae.DataContext = new AnimFile(frames);
            //    Files.ItemsSource = new List<FileAnimationEditor>() { fae };
            //    Files.SelectedIndex = 0;
            //}
            AddSpriteSheet(new SpriteSheet() { 
                Frames=frames,
                Name=spritesheetfile 
            });
        }

        private void AddSpriteSheet(SpriteSheet spriteSheet)
        {
            List<SpriteSheetVisual> lss = new List<SpriteSheetVisual>();
            SpriteSheetVisual ssv = new SpriteSheetVisual();
            ssv.DataContext = spriteSheet;
            lss.Add(ssv);
            foreach (var item in SpriteSheets.Items)
            {
                SpriteSheetVisual s = item as SpriteSheetVisual;
                if (s != null)
                {
                    lss.Add(s);
                }
            }
            SpriteSheets.ItemsSource=lss;
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #endregion Methods

        #region Image
        void SaveToBmp(FrameworkElement visual, string fileName)
        {
            var encoder = new BmpBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        void SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap(
                (int)visual.ActualWidth,
                (int)visual.ActualHeight,
                96,
                96,
                PixelFormats.Pbgra32);
            bitmap.Render(visual);
            BitmapFrame frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);

            using (var stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
        }
        #endregion Image

    }
}
