using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Records;
using UI.Interfaces;
using static Records.Record;
using Data = System.Collections.Generic.Dictionary<string, int>;

namespace UI.ViewModels
{
	public class MonthDiagrams : PropertyChangedBase, IViewModel
	{
		private readonly IExpenses expenses;

		public MonthDiagrams(IExpenses expenses)
		{
			this.expenses = expenses;

			Test = GroupByCategory(this.expenses.Records).ToDictionary(
				group => group.Key.ToString(),
				group => group.Sum(record => (int) record.Amount));
		}

		public Data Test { get; set; }

		private void Calculate(IEnumerable<Record> records)
		{
			var types = GroupByType(records).ToLookup(grouped => grouped.Key, grouped => grouped.ToArray());

			var enumerable = types[Types.Debt];
		}

		public IEnumerable<Record> FilterByMonth(IEnumerable<Record> records, int month)
		{
			return records.Where(record => record.Date.Month == month);
		}

		public IEnumerable<IGrouping<Types, Record>> GroupByType(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Type);
		}

		public IEnumerable<IGrouping<Categories, Record>> GroupByCategory(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Category);
		}

		public IEnumerable<IGrouping<string, Record>> GroupByDay(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Date.ToString("dd"))
			              .OrderBy(group => group.Key);
		}
	}
}