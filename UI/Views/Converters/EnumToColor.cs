using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Common;

namespace UI.Views.Converters
{
	public class EnumToColor : Base
	{
		private static readonly Dictionary<Record.Types, Color> colors = new Dictionary<Record.Types, Color>
		{
			[Record.Types.Expense] = Colors.Chocolate,
			[Record.Types.Debt] = Colors.SteelBlue,
			[Record.Types.Shared] = (Color) ColorConverter.ConvertFromString("#E0BB00"),
			[Record.Types.Income] = (Color) ColorConverter.ConvertFromString("#789E27")
		};

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Record.Types key;

			if (Enum.TryParse(value?.ToString(), out key) && colors.ContainsKey(key))
			{
				return new SolidColorBrush(colors[key]);
			}

			return parameter ?? Brushes.Black;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}