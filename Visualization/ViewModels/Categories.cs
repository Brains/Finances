using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker;

namespace Visualization.ViewModels
{
	// For XAML
	//------------------------------------------------------------------
	public class MarkupDictionary : Dictionary<string, int> {}

	public class Categories
	{
		//------------------------------------------------------------------
		public Dictionary<string, int> ExpencesByCategory { get; }
		public Dictionary<string, int> ExpencesByDate { get; }
		public Dictionary<string, int> ExpencesByType { get; set; }

		//------------------------------------------------------------------
		public Categories ()
		{
			ExpencesByCategory = new Dictionary<string, int>();

			var expenses = new Expenses().Records;

			ExpencesByCategory = GetExpencesByCategory(expenses);
			ExpencesByDate = GetDatesData(expenses);
			ExpencesByType = GetExpencesByType(expenses);
		}


		//------------------------------------------------------------------
		public Dictionary<string, int> GetExpencesByCategory (List<Record> expenses)
		{
			var query = from record in expenses
				group record by record.Category
				into grouped
				select new {Key = grouped.Key, Value = grouped.Sum(record => record.Amount)};

			return query.ToDictionary(x => x.Key.ToString(), x => (int) x.Value);
		}

		//------------------------------------------------------------------
		public Dictionary<string, int> GetExpencesByType (List<Record> expenses)
		{
			var query = from record in expenses
				group record by record.Type
				into grouped
				select new {Key = grouped.Key, Value = grouped.Sum(record => record.Amount)};

			return query.ToDictionary(x => x.Key.ToString(), x => (int) x.Value);
		}

		//------------------------------------------------------------------
		public Dictionary<string, int> GetDatesData (List<Record> expenses)
		{
			var query = from record in expenses
				where record.Date.Month == 3
				orderby record.Date.ToString("yy-MM-dd")
				group record by record.Date.ToString("yy-MM-dd")
				into grouped
				select new {Key = grouped.Key, Value = grouped.Sum(record => record.Amount)};

			return query.ToDictionary(x => x.Key, x => (int) x.Value);
		}
	}
}