using System;
using System.Globalization;
using System.Windows.Media;

namespace UI.Views.Converters
{
	public class DivergenceColor : Base
	{
		public int Threshold { get; } = 10;

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return  Math.Abs((decimal) value) < Threshold ? Brushes.Gray : Brushes.OrangeRed;

		}
	}
}