using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using Tracker;
using Trends;

namespace Visualization.ViewModels
{
	// For XAML
	//------------------------------------------------------------------
//	public class MarkupDictionary : Dictionary<string, int> {}

	public class Charts
	{
		private readonly IExpenses expenses;
		//------------------------------------------------------------------
		public Dictionary<string, int> ExpencesByCategory => GetExpencesByCategory(expenses.Records);
		public Dictionary<string, int> ExpencesByDate => GetDatesData(expenses.Records);
		public Dictionary<string, int> ExpencesByType => GetExpencesByType(expenses.Records);
		public Dictionary<string, int> Trend => GetTrend();

		//------------------------------------------------------------------
		public Charts (IExpenses expenses)
		{
			this.expenses = expenses;
		}


		//------------------------------------------------------------------
		public Dictionary<string, int> GetExpencesByCategory (IEnumerable<Record> records)
		{
			var query = from record in records
				group record by record.Category
				into grouped
				select new {Key = grouped.Key, Value = grouped.Sum(record => record.Amount)};

			return query.ToDictionary(x => x.Key.ToString(), x => (int) x.Value);
		}

		//------------------------------------------------------------------
		public Dictionary<string, int> GetExpencesByType (IEnumerable<Record> records)
		{
			var query = from record in records
				group record by record.Type
				into grouped
				select new {Key = grouped.Key, Value = grouped.Sum(record => record.Amount)};

			return query.ToDictionary(x => x.Key.ToString(), x => (int) x.Value);
		}

		//------------------------------------------------------------------
		public Dictionary<string, int> GetDatesData (IEnumerable<Record> records)
		{
			var query = from record in records
				where record.Date.Month == 3
				orderby record.Date.ToString("yy-MM-dd")
				group record by record.Date.ToString("yy-MM-dd")
				into grouped
				select new {Key = grouped.Key, Value = grouped.Sum(record => record.Amount)};

			return query.ToDictionary(x => x.Key, x => (int) x.Value);
		}

		//------------------------------------------------------------------
		public Dictionary<string, int> GetTrend ()
		{
			throw new NotImplementedException();
//			var trend = new Trend();
//
//			trend.LoadOperations();
//			trend.CalculateTransactionsCalendar(new LocalDate(2015, 2, 1));
//			var grouped = trend.AggregateTransactionsByDate(trend.Calendar);

//			return grouped.ToDictionary(t => t.Date.ToString("MMM/d", CultureInfo.CurrentCulture), t => (int) t.Amount);
		}
	}
}