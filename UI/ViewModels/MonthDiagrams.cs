using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Records;
using UI.Interfaces;
using static Records.Record;
using Data = System.Collections.Generic.Dictionary<string, int>;

namespace UI.ViewModels
{
	public class MonthDiagrams : Screen, IViewModel
	{
		private readonly IExpenses expenses;
		private ILookup<Types, Record> types;

		public MonthDiagrams(IExpenses expenses)
		{
			this.expenses = expenses;
		}

		public Data Test { get; set; }

		public Dictionary<string, ILookup<int, Record>> BalanceByMonth { get; set; }

		protected override void OnInitialize()
		{
			base.OnInitialize();

			types = expenses.Records.ToLookup(record => record.Type);
		}

		public IEnumerable<Record> FilterByMonth(IEnumerable<Record> records, int month)
		{
			return records.Where(record => record.Date.Month == month);
		}

		public ILookup<Types, Record> GroupByType(IEnumerable<Record> records)
		{
			return records.ToLookup(record => record.Type);
		}

		public IEnumerable<IGrouping<Categories, Record>> GroupByCategory(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Category);
		}

		public IEnumerable<IGrouping<string, Record>> GroupByDay(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Date.ToString("d"))
			              .OrderBy(group => group.Key);
		}
	}
}