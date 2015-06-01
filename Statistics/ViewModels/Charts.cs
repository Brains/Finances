using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.ObjectBuilder2;
using Statistics.Banking;
using Tracker;
using static System.Console;
using static Tracker.Record;
using static Tracker.Record.Types;
using static Tracker.Record.Categories;

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
		public Dictionary<string, int> Expences => GetExpencesByCategory(Records, IsExpense);
		public Dictionary<string, int> Incomes => GetExpencesByCategory(Records, IsIncome);
		public Dictionary<Types, int> ExpencesByType => GetInOutRatio(Records);

		public Charts(IExpenses expenses)
		{
			this.expenses = expenses;

			types = GroupByType(Records);
			funds = new Funds();
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

				var expencesTotal = new KeyValuePair<string, int>("Exp", (int) total.Sum(record => record.Amount));
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

				var expencesTotal = new KeyValuePair<string, int>("Exp", (int) total.Sum(record => record.Amount));
				var sad = new Dictionary<string, int>();
				sad.Add(expencesTotal.Key, expencesTotal.Value);

				return sad;
			}
		}

		public Dictionary<Types, int> GetInOutRatio(IEnumerable<Record> records)
		{
			var types = records.GroupBy(record => record.Type)
			                   .Where(grouping => IsThis(grouping.Key))
			                   .Select(grouping => new {grouping.Key, Value = (int) grouping.Sum(record => record.Amount)})
			                   .ToDictionary(arg => arg.Key, arg => arg.Value);

			types[Expense] += types[Shared];
			types.Remove(Shared);

			return types;
		}

		public Dictionary<Types, List<Record>> GroupByType(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Type)
			              .ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
		}

		private static bool IsThis(Types type)
		{
			return type == Expense || type == Shared || type == Income;
		}

		public Dictionary<Categories, int> CalculateDebts()
		{
			var debts = (from record in types[Debt]
			            group record by record.Category
			            into dude
			            select new
			            {
				            Name = dude.Key,
				            Total = (from record in dude
				                     group record by record.Description
				                     into grouped
				                     select new
				                     {
					                     Direction = grouped.Key,
										 Total = (int) grouped.Sum(record => record.Amount)
				                     })
									 .ToDictionary(kind => kind.Direction, kind => kind.Total)
			            });

			var total = debts.Select(dude => new
			{
				dude.Name,
				Total = dude.Total["Out"] - dude.Total["In"]
			})
			.ToDictionary(dude => dude.Name, dude => dude.Total);



			return total;
		}



	}
}