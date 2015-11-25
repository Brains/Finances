using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Finances.Converters
{
	public class ValueToFontWeight : IValueConverter
	{
		private const int Step = 100;
		static readonly List<FontWeight> Weights = new List<FontWeight> {FontWeights.Thin, FontWeights.Light, FontWeights.Normal, FontWeights.Bold,};

		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			int number;

			if (value is string)
				int.TryParse((string) value, out number);
			else
				 number = decimal.ToInt32((decimal) value);

			int index = Math.Abs(number) / Step;

			if (index >= Weights.Count)
				index = Weights.Count - 1;

			return Weights[index];
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}