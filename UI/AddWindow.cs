using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Tracker;

namespace UI
{
	internal class AddWindow
	{
		public List<Record> Records { get; set; }
		public string Amount { get; set; } = "0";
		public List<Record.Types> Types { get; set; }

		//------------------------------------------------------------------
		public AddWindow ()
		{
			Expenses expenses = new Expenses();

			Records = expenses.Records;

			Types = new List<Record.Types>() { Record.Types.Balance, Record.Types.Expense};
		}
	}



	public class CountToBackgroundConverter : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof (Brush))
				throw new NotImplementedException();


		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;

			
		}
	}
}