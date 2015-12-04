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

		public IEnumerable<IGrouping<Types, Record>> GroupByType(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Type);
		}

		{
			var query = from record in records
						group record by record.Type
						into grouped
						select new { Key = grouped.Key, Value = grouped.Sum(record => record.Amount) };

			return query.ToDictionary(x => x.Key.ToString(), x => (int) x.Value);
		}
	}
}