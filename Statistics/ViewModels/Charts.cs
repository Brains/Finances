using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Tracker;
using static Tracker.Record;
using static Tracker.Record.Types;
using Data = System.Collections.Generic.Dictionary<string, int>;

namespace Statistics.ViewModels
{
	// For XAML
//	public class MarkupDictionary : Dictionary<string, int> {}

	public class Charts
	{
		private readonly IExpenses expenses;
		private int month = DateTime.Now.Month-1;
		private Dictionary<Types, List<Record>> types;
		private readonly Funds funds;

		private IEnumerable<Record> Records => GetRecordsByMonth(expenses.Records, month);
		public string Month => DateTimeFormatInfo.CurrentInfo.GetMonthName(month);

		// View Bindings
		public Dictionary<string, IEnumerable<Record>> ExpencesByDate => GetExpencesByDate(Records);
		public Data Expences => GetExpencesByCategory(Records, IsSpending);
		public Data Incomes => GetExpencesByCategory(Records, IsIncome);
		public Data ExpencesByType => CalculateInOut();

		public Charts(IExpenses expenses)
		{
			this.expenses = expenses;

			types = GroupByType(Records);
		}

		public Dictionary<string, IEnumerable<Record>> GetExpencesByDate(IEnumerable<Record> records)
		{
			var dates = from record in records
			            where IsSpending(record)
			            orderby record.Date.Date.ToString(CultureInfo.InvariantCulture)
			            group record by record.Date.ToString("dd")
			            into grouped
			            select grouped;

			return dates.ToDictionary(date => date.Key, AggregateByCategory);
		}

		public Data GetExpencesByCategory(IEnumerable<Record> records, Predicate<Record> predicate)
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
//			            where record.Type == Expense || record.Type == Income
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
			var asdd = records.ToLookup(record => record.Type, record => records.Where(record1 => record.Amount > 2));

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
				["Spending"] = (int) (types[Expense].Concat(types[Shared]).Sum(amount)),
				["Income"] = (int) types[Income].Sum(amount),
			};
		}


		



	}
}