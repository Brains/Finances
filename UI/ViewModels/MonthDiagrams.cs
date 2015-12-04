using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Records;
using UI.Interfaces;
using static Records.Record;

namespace UI.ViewModels
{
	public class MonthDiagrams : PropertyChangedBase, IViewModel
	{
		private IExpenses expenses;

		public MonthDiagrams(IExpenses expenses)
		{
			this.expenses = expenses;
		}

		private void Calculate(IEnumerable<Record> records)
		{
			var types = GroupByType(records).ToLookup(grouped => grouped.Key, grouped => grouped.ToArray());

			var enumerable = types[Types.Debt];
		}

		public IEnumerable<IGrouping<Types, Record>> GroupByType(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Type);
		}

		public IEnumerable<IGrouping<Categories, Record>> GroupByCategory(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Category);
		}

		public IEnumerable<Record> FilterByMonth(IEnumerable<Record> records, int month)
		{
			return records.Where(record => record.Date.Month == month);

		}
	}
}