using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Common;
using Common.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using static Common.Record;
using static Common.Record.Types;
using Data = System.Collections.Generic.Dictionary<string, int>;

namespace Statistics.ViewModels
{
	// For XAML
//	public class MarkupDictionary : Dictionary<string, int> {}

	public class Charts : BindableBase
	{
		private readonly IExpenses expenses;
		private int month = DateTime.Now.Month;
		private Dictionary<Types, List<Record>> types;

		private IEnumerable<Record> Records { get; set; }
		public string Month { get; set; }

		// View Bindings
		public Dictionary<string, IEnumerable<Record>> SpendingByDate { get; set; }
		public Data Spending { get; set; }
		public Data Incomes { get; set; }
		public Data SpendingByType { get; set; }
		public DelegateCommand NextMonth { get; set; }
		public DelegateCommand PreviousMonth { get; set; }

		public Charts(IExpenses expenses, IEventAggregator eventAggregator)
		{
			this.expenses = expenses;
			eventAggregator.GetEvent<AddRecord>().Subscribe(record => Update(month));

			NextMonth = new DelegateCommand(() => ShiftMonth(1), CanSelectNextMonth);
			PreviousMonth = new DelegateCommand(() => ShiftMonth(-1), CanSelectPreviousMonth);

			Update(month);
		}

		public void Update(int selectedMonth)
		{
			Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
			OnPropertyChanged("Month");

			Records = GetRecordsByMonth(expenses.Records, selectedMonth).ToList();
			types = GroupByType(Records);

			SpendingByDate = GetSpendingByDate(Records);
			Spending = GetSpendingByCategory(Records, IsSpending);
			Incomes = GetSpendingByCategory(Records, IsIncome);
			SpendingByType = CalculateInOut();
			OnPropertyChanged("SpendingByDate");
			OnPropertyChanged("Spending");
			OnPropertyChanged("Incomes");
			OnPropertyChanged("SpendingByType");
		}

		public Dictionary<string, IEnumerable<Record>> GetSpendingByDate(IEnumerable<Record> records)
		{
			var dates = from record in records
			            where IsSpending(record)
			            orderby record.Category
			            group record by record.Date.ToString("dd")
			            into grouped
						orderby grouped.Key
                        select grouped;

			return dates.ToDictionary(date => date.Key, AggregateByCategory);
		}

		public Data GetSpendingByCategory(IEnumerable<Record> records, Predicate<Record> predicate)
		{
			var query = from record in records
			            where predicate(record)
			            orderby record.Category
			            group record by record.Category
			            into grouped
			            select grouped;

			return query.ToDictionary(group => group.Key.ToString(), group => (int) group.Sum(record => record.Amount));
		}


		public Data GetExpencesByType(IEnumerable<Record> records)
		{
			var query = from record in records
			            group record by record.Type
			            into grouped
			            select new {Key = grouped.Key, Value = grouped.Sum(record => record.Amount)};

			return query.ToDictionary(x => x.Key.ToString(), x => (int) x.Value);
		}

		#region Retrieve Helpers

		public Dictionary<Types, List<Record>> GroupByType(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Type)
			              .ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
		}

		public List<Record> GetRecordsFrom(DateTime date, IEnumerable<Record> records)
		{
			var query = from record in records
			            where record.Date > date
			            select record;

			return query.ToList();
		}

		private IEnumerable<Record> GetRecordsByMonth(IEnumerable<Record> records, int month)
		{
			return records.Where(record => record.Date.Month == month);
		}

		private static bool IsSpending(Record record)
		{
			return record.Type == Expense || record.Type == Shared;
		}

		private static bool IsIncome(Record record)
		{
			return record.Type == Income;
		}

		#endregion

		private static IEnumerable<Record> AggregateByCategory(IGrouping<string, Record> date)
		{
			return from record in date
			       orderby record.Category
			       group record by record.Category
			       into grouped
			       select grouped.Aggregate((a, b) => a + b);
		}

		public Data CalculateInOut()
		{
			Func<Record, decimal> amount = record => record.Amount;

			return new Data
			{
				["Spending"] = (int) Records.Where(record => record.Type == Expense)
				                            .Concat(Records.Where(record => record.Type == Shared))
											.Sum(amount),

				["Income"] = (int) Records.Where(record => record.Type == Income).Sum(amount),
			};
		}

		#region Month Selection

		private void ShiftMonth(int shift)
		{
			var year = DateTime.Now.Year;
			DateTime date = new DateTime(year, month, 1);

			month = date.AddMonths(shift).Month;

			NextMonth.RaiseCanExecuteChanged();
			PreviousMonth.RaiseCanExecuteChanged();

			Update(month);
		}

		private bool CanSelectNextMonth()
		{
			var last = expenses.Records.Where(IsSpending).Last().Date.Month;

			if (month < last)
				return true;

			return false;
		}

		private bool CanSelectPreviousMonth()
		{
			var first = expenses.Records.Where(IsSpending).First().Date.Month;
			if (month > first)
				return true;

			return false;
		}

		#endregion
	}
}