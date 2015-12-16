using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Common;
using Common.Storages;
using UI.Interfaces;
using static System.Globalization.DateTimeFormatInfo;
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

			Month = DateTime.Now.ToString("MMMM");
        }

		public Dictionary<int, CategoryData[]> ExpenseByDay { get; private set; }
		public Dictionary<Categories, decimal> ExpenseByCategory { get; private set; }
		public Dictionary<Categories, decimal> IncomeByCategory { get; private set; }
		public Dictionary<Types, Dictionary<string, decimal>> BalanceByMonth { get; private set; }
		public Dictionary<string, CategoryData[]> ExpenseByMonth { get; set; }
		public string Month { get; set; }

		protected override void OnInitialize()
		{
			base.OnInitialize();

			Update();
		}

		public void Update()
		{
			types = expenses.Records.ToLookup(record => record.Type);
			var expense = types[Expense].Concat(types[Shared]).ToArray();

			ExpenseByMonth = Group(expense, record => record.Date.Month)
				.ToDictionary(month => CurrentInfo.GetMonthName(month.Key), pair => pair.Value);

			ExpenseByDay = Group(expense, record => record.Date.Day);
			ExpenseByCategory = GroupByCategory(expense);
			IncomeByCategory = GroupByCategory(types[Income]);
			BalanceByMonth = CalculateBalanceByMonth(types);

			NotifyOfPropertyChange(nameof(BalanceByMonth));
			NotifyOfPropertyChange(nameof(ExpenseByCategory));
			NotifyOfPropertyChange(nameof(IncomeByCategory));
			NotifyOfPropertyChange(nameof(ExpenseByDay));
		}

		public Dictionary<int, CategoryData[]> Group(IEnumerable<Record> records, Func<Record, int> selector)
		{
			return records.GroupBy(selector)
			              .OrderBy(grouping => grouping.Key)
			              .ToDictionary(grouping => grouping.Key,
			                            grouping => grouping.GroupBy(record => record.Category)
			                                                .Select(SquashRecords)
			                                                .ToArray());
		}

		public Dictionary<Categories, decimal> GroupByCategory(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Category)
			              .ToDictionary(group => group.Key,
			                            group => group.Sum(record => record.Amount));
		}

		public Dictionary<Types, Dictionary<string, decimal>> CalculateBalanceByMonth(ILookup<Types, Record> records)
		{
			return new Dictionary<Types, Dictionary<string, decimal>>
			{
				[Expense] = CalculateTotalByMonth(records[Expense].Concat(records[Shared])),
				[Income] = CalculateTotalByMonth(records[Income])
			};
		}

		public Dictionary<string, decimal> CalculateTotalByMonth(IEnumerable<Record> records)
		{
			return records.GroupBy(record => record.Date.ToString("MMMM"))
			              .ToDictionary(group => group.Key,
			                            group => group.Sum(record => record.Amount));
		}

		public CategoryData SquashRecords(IGrouping<Categories, Record> group)
		{
			return new CategoryData
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

		public class CategoryData
		{
			public Categories Category { get; set; }
			public decimal Amount { get; set; }
			public string Description { get; set; }
		}
	}
}