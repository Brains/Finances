using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.ObjectBuilder2;
using Tracker;
using static System.Console;
using static Tracker.Record;
using static Tracker.Record.Types;
using static Tracker.Record.Categories;

namespace Visualization.ViewModels
{
	// For XAML
//	public class MarkupDictionary : Dictionary<string, int> {}

	public class Charts
	{
		private readonly IExpenses expenses;
		private int month = DateTime.Now.Month;

		private IEnumerable<Record> Records => GetRecordsByMonth(expenses.Records, month);
		public string Month => DateTimeFormatInfo.CurrentInfo.GetMonthName(month);

		// View Bindings
		public Dictionary<string, IEnumerable<Record>> ExpencesByDate => GetExpencesByDate(Records);
		public Dictionary<string, int> Expences => GetExpencesByCategory(Records, IsExpense);
		public Dictionary<string, int> Incomes => GetExpencesByCategory(Records, IsIncome);
		public Dictionary<string, int> ExpencesByType => GetExpencesByType(Records);

		public Charts(IExpenses expenses)
		{
			this.expenses = expenses;
		}

		public Dictionary<string, IEnumerable<Record>> GetExpencesByDate(IEnumerable<Record> records)
		{
			var dates = from record in records
			            where IsExpense(record)
			            orderby record.Date.Date.ToString(CultureInfo.InvariantCulture)
			            group record by record.Date.ToString("dd")
			            into grouped
			            select grouped;

			return dates.ToDictionary(date => date.Key, AggregateByCategory);
		}

		public Dictionary<string, int> GetExpencesByCategory(IEnumerable<Record> records, Predicate<Record> predicate)
		{
			var query = from record in records
			            where predicate(record)
			            orderby record.Category
			            group record by record.Category
			            into grouped
			            select grouped;

			return query.ToDictionary(group => group.Key.ToString(), group => (int) group.Sum(record => record.Amount));
		}


		public Dictionary<string, int> GetExpencesByType(IEnumerable<Record> records)
		{
			var query = from record in records
//			            where record.Type == Expense || record.Type == Income
			            group record by record.Type
			            into grouped
			            select new {Key = grouped.Key, Value = grouped.Sum(record => record.Amount)};

			return query.ToDictionary(x => x.Key.ToString(), x => (int) x.Value);
		}

		#region Retrieve Helpers

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

		#endregion

		private static IEnumerable<Record> AggregateByCategory(IGrouping<string, Record> date)
		{
			return from record in date
			       orderby record.Category
			       group record by record.Category
			       into grouped
			       select grouped.Aggregate((a, b) => a + b);
		}

		private static bool IsExpense(Record record)
		{
			return record.Type == Expense || record.Type == Shared;
		}

		private static bool IsIncome(Record record)
		{
			return record.Type == Income;
		}


		public Dictionary<string, int> ExpencesTotal
		{
			get
			{
				var total = from record in Records
				            where IsExpense(record)
				            select record;

				var expencesTotal = new KeyValuePair<string, int>("Exp", (int)total.Sum(record => record.Amount));
				var sad = new Dictionary<string, int>(); 
				sad.Add(expencesTotal.Key, expencesTotal.Value);

				return sad;
			}
		}


		public Dictionary<string, int> IncomesTotal
		{
			get
			{
				var total = from record in Records
							where IsIncome(record)
							select record;

				var expencesTotal = new KeyValuePair<string, int>("Exp", (int)total.Sum(record => record.Amount));
				var sad = new Dictionary<string, int>();
				sad.Add(expencesTotal.Key, expencesTotal.Value);

				return sad;
			}
		}
	}
}