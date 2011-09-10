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
	/// Interaction logic for EditFrame.xaml
	/// </summary>
	public partial class EditFrame : UserControl
	{
		public EditFrame()
		{
			this.InitializeComponent();
		}

        public EditFrame(Frame f)
        {
            DataContext = f;
            this.InitializeComponent();
        }
	}
}