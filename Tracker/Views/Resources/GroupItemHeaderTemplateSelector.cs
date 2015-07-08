using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Tracker.Views.Resources
{
	public class GroupItemHeaderTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			CollectionViewGroup group = item as CollectionViewGroup;
			Window window = Application.Current.MainWindow;

			DataTemplate template = window.FindResource("GroupHeaderTemplateDay") as DataTemplate;

			if (!group.IsBottomLevel)
				template = window.FindResource("GroupHeaderTemplateMonth") as DataTemplate;

			return template;
		}
	}
}