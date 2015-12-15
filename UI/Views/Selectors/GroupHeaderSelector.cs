using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace UI.Views.Selectors
{
	public class GroupHeaderSelector : DataTemplateSelector
	{
		public DataTemplate DayHeader { get; set; }
		public DataTemplate MonthHeader { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			CollectionViewGroup group = item as CollectionViewGroup;

			return group.IsBottomLevel ? DayHeader : MonthHeader;
		}
	}
}