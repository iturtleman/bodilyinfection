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

namespace AnimationEditor
{
	/// <summary>
	/// Interaction logic for SpriteSheetVisual.xaml
	/// </summary>
	public partial class SpriteSheetVisual : UserControl
	{
		public SpriteSheetVisual()
		{
			this.InitializeComponent();
		}
	}

    public class SpriteSheetConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<Frame> ss = value as List<Frame>;
            if (ss != null && ss.Count > 0)
            {
                return ss[0].Image;
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class NameConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string name = value as string;
            return name.Substring(name.LastIndexOf('\\')+1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}