using System;
using System.Globalization;
using System.Windows.Data;

namespace UI.Views.Converters
{
	public class GroupingDate : Base
	{
		public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((DateTime) value).ToString("d MMMM, dddd");
		}
	}
}
