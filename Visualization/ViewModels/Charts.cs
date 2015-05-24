using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tracker;

namespace Visualization.ViewModels
{
	// For XAML
	
//	public class MarkupDictionary : Dictionary<string, int> {}

	public class Charts
	{
		private readonly IExpenses expenses;

		public Dictionary<string, int> ExpencesByCategory => GetExpencesByCategory(GetRecordsByMonth(expenses.Records, DateTime.Now.Month));
		public List<IGrouping<string, Record>> ExpencesByDate => GetRecordsGroupedByDate(GetRecordsByMonth(expenses.Records, DateTime.Now.Month));
		public Dictionary<string, int> ExpencesByType => GetExpencesByType(expenses.Records);
		
		public Charts (IExpenses expenses)
		{
			this.expenses = expenses;
		}

		public Dictionary<string, int> GetExpencesByCategory (IEnumerable<Record> records)
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

		public Dictionary<string, int> GetExpencesByType (IEnumerable<Record> records)
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
		
		public List<Record> GetRecordsFrom (DateTime date, IEnumerable<Record> records)
		{
			var query = from record in records
				where record.Date > date
				select record;

			return query.ToList();
		}
		
		private List<IGrouping<string, Record>> GetRecordsGroupedByDate(IEnumerable<Record> records)
		{
			var format = "dd";

			var query = from record in records
				orderby record.Date.Date.ToString(CultureInfo.InvariantCulture)
				group record by record.Date.ToString(format)
				into grouped
				select grouped;

			return query.ToList();
		}
	}
}