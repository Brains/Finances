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
		private readonly IAnalyzer analyzer;
		private ILookup<Types, Record> types;

		public MonthDiagrams(IExpenses expenses, IAnalyzer analyzer)
		{
			this.expenses = expenses;
			this.analyzer = analyzer;

			Update();
		}

		public Data Test { get; set; }

		public Dictionary<Types, Dictionary<int, decimal>> BalanceByMonth { get; set; }

		public void Update()
		{
			types = expenses.Records.ToLookup(record => record.Type);

			BalanceByMonth = CalculateBalanceByMonth();
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