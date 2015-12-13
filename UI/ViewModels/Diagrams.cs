using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Common;
using Common.Storages;
using UI.Interfaces;
using static Common.Record;
using static Common.Record.Types;

namespace UI.ViewModels
{
	public class Diagrams : Screen, IViewModel
	{
		private readonly IExpenses expenses;

		private ILookup<Types, Record> types;

		public Diagrams(IExpenses expenses)
		{
			this.expenses = expenses;
		}

		public Dictionary<Types, Dictionary<int, decimal>> BalanceByMonth { get; private set; }
		public Dictionary<Categories, decimal> ExpenseByCategory { get; private set; }
		public Dictionary<Categories, decimal> IncomeByCategory { get; private set; }
		public Dictionary<string, Data[]> ExpenseByDay { get; private set; }

		protected override void OnInitialize()
		{
			base.OnInitialize();

			Update();
		}

		public void Update()
		{
			types = expenses.Records.ToLookup(record => record.Type);
			var expense = types[Expense].Concat(types[Shared]).ToArray();

			ExpenseByDay = GroupByDay(expense);
			ExpenseByCategory = GroupByCategory(expense);
			IncomeByCategory = GroupByCategory(types[Income]);
			BalanceByMonth = CalculateBalanceByMonth();

			NotifyOfPropertyChange(nameof(BalanceByMonth));
			NotifyOfPropertyChange(nameof(ExpenseByCategory));
			NotifyOfPropertyChange(nameof(IncomeByCategory));
			NotifyOfPropertyChange(nameof(ExpenseByDay));
		}

		private Dictionary<string, Data[]> GroupByDay(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Date.ToString("%d"))
			              .ToDictionary(day => day.Key,
			                            day => day.GroupBy(record => record.Category)
			                                      .Select(SquashRecords)
			                                      .ToArray());
		}

		private Dictionary<Categories, decimal> GroupByCategory(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Category)
			              .ToDictionary(group => group.Key,
			                            group => group.Sum(record => record.Amount));
		}

		private Dictionary<Types, Dictionary<int, decimal>> CalculateBalanceByMonth()
		{
			return new Dictionary<Types, Dictionary<int, decimal>>
			{
				[Expense] = CalculateTotalByMonth(types[Expense].Concat(types[Shared])),
				[Income] = CalculateTotalByMonth(types[Income])
			};
		}

		public Dictionary<int, decimal> CalculateTotalByMonth(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Date.Month)
			              .ToDictionary(group => group.Key,
			                            group => group.Sum(record => record.Amount));
		}

		private Data SquashRecords(IGrouping<Categories, Record> group)
		{
			return new Data
			{
				Category = group.Key,
				Amount = group.Sum(r => r.Amount),
				Description = group.Select(r => r.Description)
				                   .Aggregate((a, b) => $"{a}\n{b}")
			};
		}

		public IEnumerable<Record> FilterByMonth(IEnumerable<Record> records, int month)
		{
			return records.Where(record => record.Date.Month == month);
		}

		public class Data
		{
			public Categories Category { get; set; }
			public decimal Amount { get; set; }
			public string Description { get; set; }
		}
	}
}