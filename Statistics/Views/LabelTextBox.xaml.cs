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

namespace Statistics.Views
{
	/// <summary>
	/// Interaction logic for LabelTextBox.xaml
	/// </summary>
	public partial class LabelTextBox : UserControl
	{
		public LabelTextBox()
		{
			InitializeComponent();
		}



		string localLabel = "";
		string localTextBox = "";

		public string Label
		{
			get { return localLabel; }
			set
			{
				localLabel = value;
				BaseLabel.Content = value;
			}
		}

		public string TextBox
		{
			get { return localTextBox; }
			set
			{
				localTextBox = value;
				BaseTextBox.Text = value;
			}
		}
	}
}