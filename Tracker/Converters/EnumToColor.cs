using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Tracker.Converters
{
	public class EnumToColor : IValueConverter
	{
		private static readonly Dictionary<string, Brush> Colors;

		//------------------------------------------------------------------
		static EnumToColor ()
		{
			Colors = new Dictionary<string, Brush>
			{
				["Expense"] = (Brush) Application.Current.FindResource("ExpenseColor"),
				["Income"] = (Brush) Application.Current.FindResource("IncomeColor"),
				["Balance"] = (Brush) Application.Current.FindResource("BalanceColor"),
				["Debt"] = (Brush) Application.Current.FindResource("DebtColor"),
			};
		}

		//------------------------------------------------------------------
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Colors[value.ToString()];
		}

		//------------------------------------------------------------------
		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}