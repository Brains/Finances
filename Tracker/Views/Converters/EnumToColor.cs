using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Tracker.Views.Converters
{
	public class EnumToColor : IValueConverter
	{
		private static readonly Dictionary<string, Brush> Colors;

		static EnumToColor ()
		{
			Colors = new Dictionary<string, Brush>
			{
				["Default"] = (Brush) Application.Current.FindResource("DefaultColor"),
				["Expense"] = (Brush) Application.Current.FindResource("ExpenseColor"),
				["Income"] = (Brush) Application.Current.FindResource("IncomeColor"),
				["Balance"] = (Brush) Application.Current.FindResource("BalanceColor"),
				["Shared"] = (Brush) Application.Current.FindResource("SharedColor"),
				["Debt"] = (Brush) Application.Current.FindResource("DebtColor"),
			};
		}

		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return Colors["Default"];

			var key = value.ToString();

			if (Colors.ContainsKey(key))
				return Colors[key];

			return Colors["Default"];
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}