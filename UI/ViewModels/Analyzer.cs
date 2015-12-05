using System.Collections.Generic;
using System.Linq;
using Records;
using static Records.Record;

namespace UI.ViewModels
{
	public interface IAnalyzer
	{
		ILookup<Types, Record> GroupByType(IEnumerable<Record> records);
		IEnumerable<Record> FilterByMonth(IEnumerable<Record> records, int month);
		IEnumerable<IGrouping<Categories, Record>> GroupByCategory(IEnumerable<Record> records);
		IEnumerable<IGrouping<string, Record>> GroupByDay(IEnumerable<Record> records);
		Dictionary<int, decimal> CalculateTotalByMonth(IEnumerable<Record> records);
	}

	public class Analyzer : IAnalyzer
	{
		public ILookup<Types, Record> GroupByType(IEnumerable<Record> records)
		{
			return records.ToLookup(record => record.Type);
		}

		public IEnumerable<Record> FilterByMonth(IEnumerable<Record> records, int month)
		{
			return records.Where(record => record.Date.Month == month);
		}

		public IEnumerable<IGrouping<Categories, Record>> GroupByCategory(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Category);
		}

		public IEnumerable<IGrouping<string, Record>> GroupByDay(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Date.ToString("%d"))
			              .OrderBy(group => group.Key);
		}

		public Dictionary<int, decimal> CalculateTotalByMonth(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Date.Month).ToDictionary(
				group => group.Key,
				group => group.Sum(record => record.Amount));
		}
	}
}