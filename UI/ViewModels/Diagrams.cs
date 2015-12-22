using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		private int month;

		public Diagrams() : this(new FixedRecords())
		{
			Update();
		}

		public Diagrams(IExpenses expenses)
		{
			this.expenses = expenses;

			month = DateTime.Now.Month - 2;
        }

		public Dictionary<int, CategoryData[]> ExpenseByDay { get; private set; }
		public Dictionary<Categories, decimal> ExpenseByCategory { get; private set; }
		public Dictionary<Categories, decimal> IncomeByCategory { get; private set; }
		public Dictionary<Types, Dictionary<string, decimal>> BalanceByMonth { get; private set; }
		public Dictionary<string, CategoryData[]> ExpenseByMonth { get; set; }

		public string Month => CurrentInfo.GetMonthName(month);

		protected override void OnInitialize()
		{
			base.OnInitialize();

			Update();
		}

		public void Update()
		{
			UpdateYearly();
			UpdateMonthly();
		}

		private void UpdateYearly()
		{
			types = expenses.Records.ToLookup(record => record.Type);
			var expense = types[Expense].Concat(types[Shared]).ToArray();

			ExpenseByMonth = Group(expense, record => record.Date.Month)
				.ToDictionary(month => CurrentInfo.GetMonthName(month.Key), pair => pair.Value);
			BalanceByMonth = CalculateBalanceByMonth(types);

			NotifyOfPropertyChange(nameof(BalanceByMonth));
			NotifyOfPropertyChange(nameof(ExpenseByMonth));
		}

		private void UpdateMonthly()
		{
			types = expenses.Records.Where(record => record.Date.Month == month)
			                .ToLookup(record => record.Type);
			var expense = types[Expense].Concat(types[Shared]).ToArray();

			ExpenseByDay = Group(expense, record => record.Date.Day);
			ExpenseByCategory = GroupByCategory(expense);
			IncomeByCategory = GroupByCategory(types[Income]);

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
			return records.GroupBy(record => record.Date.Month)
			              .OrderBy(month => month.Key)
			              .ToDictionary(month => CurrentInfo.GetMonthName(month.Key),
										month => month.Sum(record => record.Amount));

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