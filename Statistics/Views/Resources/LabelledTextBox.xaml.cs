using System.Windows;

namespace Statistics.Views.Resources
{
	public partial class LabelledTextBox
	{
		public static readonly DependencyProperty LabelProperty = DependencyProperty
			.Register("Label",
					typeof(string),
					typeof(LabelledTextBox),
					new FrameworkPropertyMetadata("Default"));

		public static readonly DependencyProperty TextProperty = DependencyProperty
			.Register("Text",
					typeof(string),
					typeof(LabelledTextBox),
					new FrameworkPropertyMetadata("Default", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public LabelledTextBox()
		{
			InitializeComponent();
			Root.DataContext = this;
		}

		public string Label
		{
			get { return (string) GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}

		public string Text
		{
			get { return (string) GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
	}
}