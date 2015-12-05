using System;
using System.Collections.ObjectModel;
using System.Linq;
using static Records.Record.Types;
using static Records.Record.Categories;

namespace Records
{
	public class FixedRecords : IExpenses, IRecordsStorage
	{
		public static readonly Record[] Data =
		{
			// October
			new Record(100, Expense,Food,   "Novus",    new DateTime(2015, 10, 1)),
			new Record(100, Income, Deposit,"",         new DateTime(2015, 10, 2)),
			new Record(100, Shared, House,  "O3",       new DateTime(2015, 10, 3)),
			new Record(100, Debt,   Maxim,  "Out",      new DateTime(2015, 10, 4)),
			new Record(100, Expense,Health, "Pharmacy", new DateTime(2015, 10, 5)),

			// November
			new Record(100, Expense,Food,   "Novus",    new DateTime(2015, 11, 1)),
			new Record(100, Income, Deposit,"",         new DateTime(2015, 11, 2)),
			new Record(100, Shared, House,  "O3",       new DateTime(2015, 11, 3)),
			new Record(100, Debt,   Maxim,  "Out",      new DateTime(2015, 11, 4)),
			new Record(100, Expense,Health, "Pharmacy", new DateTime(2015, 11, 5)),

			// December
			new Record(100, Expense,Food,   "Novus",    new DateTime(2015, 12, 1)),
			new Record(100, Income, Deposit,"",         new DateTime(2015, 12, 2)),
			new Record(100, Shared, House,  "O3",       new DateTime(2015, 12, 3)),
			new Record(100, Debt,   Maxim,  "Out",      new DateTime(2015, 12, 4)),
			new Record(100, Expense,Health, "Pharmacy", new DateTime(2015, 12, 5)),
		};


		public ObservableCollection<Record> Records { get; set; }

		public FixedRecords()
		{
			Records = new ObservableCollection<Record>(Data);
		}

		public void Add(Record record) => Records.Add(record);

	}
}