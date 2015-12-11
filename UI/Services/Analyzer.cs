using System.Collections.Generic;
using System.Linq;
using Common;

namespace UI.Services
{
	public interface IAnalyzer
	{
		ILookup<Record.Types, Record> GroupByType(IEnumerable<Record> records);
		IEnumerable<Record> FilterByMonth(IEnumerable<Record> records, int month);
		ILookup<Record.Categories, Record> GroupByCategory(IEnumerable<Record> records);
		ILookup<string, Record> GroupByDay(IEnumerable<Record> records);
		Dictionary<int, decimal> CalculateTotalByMonth(IEnumerable<Record> records);
	}

	public class Analyzer : IAnalyzer
	{
		public ILookup<Record.Types, Record> GroupByType(IEnumerable<Record> records)
		{
			return records.ToLookup(record => record.Type);
		}

		public IEnumerable<Record> FilterByMonth(IEnumerable<Record> records, int month)
		{
			return records.Where(record => record.Date.Month == month);
		}

		public ILookup<Record.Categories, Record> GroupByCategory(IEnumerable<Record> records)
		{
			return records.ToLookup(record => record.Category);
		}

		public ILookup<string, Record> GroupByDay(IEnumerable<Record> records)
		{
			return records.ToLookup(record => record.Date.ToString("%d"));
;		}

		public Dictionary<int, decimal> CalculateTotalByMonth(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Date.Month).ToDictionary(
				group => group.Key,
				group => group.Sum(record => record.Amount));
		}
	}
}