using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media;
using Common;
// ReSharper disable PossibleNullReferenceException

namespace UI.Views.Converters
{
	public class EnumToColor : Base
	{
		private static readonly Dictionary<Record.Types, Color> colors = new Dictionary<Record.Types, Color>
		{
			[Record.Types.Expense] =(Color) ColorConverter.ConvertFromString("#BA55A6"),
			[Record.Types.Debt] =	(Color) ColorConverter.ConvertFromString("#6685A2"),
			[Record.Types.Shared] = (Color) ColorConverter.ConvertFromString("#61B7EB"),
			[Record.Types.Income] = (Color) ColorConverter.ConvertFromString("#8FCD3E")
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
	}
}