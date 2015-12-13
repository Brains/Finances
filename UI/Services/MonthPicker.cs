using System;
using System.Globalization;

namespace UI.Services
{
	public class MonthPicker
	{
		private int month = DateTime.Now.Month;

		public MonthPicker()
		{
			Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);

		}
		public string Month { get; set; }

		private void Shift(int shift)
		{
			var year = DateTime.Now.Year;
			DateTime date = new DateTime(year, month, 1);

			month = date.AddMonths(shift).Month;

//			NextMonth.RaiseCanExecuteChanged();
//			PreviousMonth.RaiseCanExecuteChanged();
//
//			Update(month);
		}



	}
}