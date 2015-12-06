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
		private readonly IAnalyzer analyzer;
		private readonly IExpenses expenses;
		private ILookup<Types, Record> types;

		public MonthDiagrams(IExpenses expenses, IAnalyzer analyzer)
		{
			this.expenses = expenses;
			this.analyzer = analyzer;
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();

			Update();
		}

		public Data Test { get; set; }

		public Dictionary<Types, Dictionary<int, decimal>> BalanceByMonth { get; set; }
		public Dictionary<Categories, decimal> ExpenseByCategory { get; set; }
		public Dictionary<Categories, decimal> IncomeByCategory { get; set; }

		public void Update()
		{
			types = analyzer.GroupByType(expenses.Records);

			BalanceByMonth = CalculateBalanceByMonth();
			ExpenseByCategory = GroupByCategory(types[Types.Expense].Concat(types[Types.Shared]));
			IncomeByCategory = GroupByCategory(types[Types.Income]);
		}

		private Dictionary<Categories, decimal> GroupByCategory(IEnumerable<Record> records)
		{
			return analyzer.GroupByCategory(records).ToDictionary(
				grouping => grouping.Key,
				grouping => grouping.Sum(record => record.Amount));
		}

		private Dictionary<Types, Dictionary<int, decimal>> CalculateBalanceByMonth()
		{
			return new Dictionary<Types, Dictionary<int, decimal>>
			{
				[Types.Expense] = analyzer.CalculateTotalByMonth(types[Types.Expense].Concat(types[Types.Shared])),
				[Types.Income] = analyzer.CalculateTotalByMonth(types[Types.Income])
			};
		}
	}
}