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

namespace Visualization.ViewModels
{
	// For XAML

//	public class MarkupDictionary : Dictionary<string, int> {}

	public class Charts
	{
		private readonly IExpenses expenses;

		public Dictionary<string, int> ExpencesByCategory
			=> GetExpencesByCategory(GetRecordsByMonth(expenses.Records, DateTime.Now.Month));

//		public List<IGrouping<string, Record>> ExpencesByDate
//			=> GetRecordsGroupedByDate(GetRecordsByMonth(expenses.Records, DateTime.Now.Month));

		public Dictionary<string, int> ExpencesByType => GetExpencesByType(expenses.Records);

		public Charts(IExpenses expenses)
		{
			this.expenses = expenses;
		}

		public Dictionary<string, int> GetExpencesByCategory(IEnumerable<Record> records)
		{
			var query = from record in records
				group record by record.Category
				into grouped
				select new {Key = grouped.Key, Value = grouped.Sum(record => record.Amount)};

			return query.ToDictionary(x => x.Key.ToString(), x => (int) x.Value);
		}

		private IEnumerable<Record> GetRecordsByMonth(IEnumerable<Record> records, int month)
		{
			return records.Where(record => record.Date.Month == month);
		}

		public Dictionary<string, int> GetExpencesByType(IEnumerable<Record> records)
		{
			var query = from record in records
				group record by record.Type
				into grouped
				select new {Key = grouped.Key, Value = grouped.Sum(record => record.Amount)};

			return query.ToDictionary(x => x.Key.ToString(), x => (int) x.Value);
		}

//		public Dictionary<string, int> GetDatesData (IEnumerable<Record> records)
//		{
//			var query = from record in records
//				where record.Date.Month == 3
//				orderby record.Date.ToString("yy-MM-dd")
//				group record by record.Date.ToString("yy-MM-dd")
//				into grouped
//				select new {Key = grouped.Key, Value = grouped.Sum(record => record.Amount)};
//
//			return query.ToDictionary(x => x.Key, x => (int) x.Value);
//		}

		public List<Record> GetRecordsFrom(DateTime date, IEnumerable<Record> records)
		{
			var query = from record in records
				where record.Date > date
				select record;

			return query.ToList();
		}

		public void GetRecordsGroupedByDate(IEnumerable<Record> records)
		{
			var format = "dd";

			var types = Record.Types.Shared;

			records.Where(record => record.Type == types).ForEach(WriteLine);
			WriteLine();

			var dates = from record in records
				where record.Type == types
				orderby record.Date.Date.ToString(CultureInfo.InvariantCulture)
				group record by record.Date.ToString(format)
				into grouped
				select grouped;

			dates.ForEach(grouping =>
			{
				WriteLine(grouping.Key);
				grouping.ForEach(record => WriteLine("  " + record) );
			});
			WriteLine();


			foreach (var date in dates)
			{
				WriteLine(date.Key);
				GetCategizedRecords(date);
			}


		}

		private void  GetCategizedRecords(IGrouping<string, Record> date)
		{
			var result = from record in date
				orderby record.Category
				group record by record.Category
				into grouped
				select grouped;

			result.ForEach(grouping =>
			{
				var agr = grouping.Aggregate((a, b) => 
				new Record(a.Amount + b.Amount, a.Type, grouping.Key, $"{a.Description}, {b.Description}", DateTime.Now));

				WriteLine($"  {grouping.Key} : {agr}");
				grouping.ForEach(record => WriteLine("\t" + record));
			});


		}
	}
}