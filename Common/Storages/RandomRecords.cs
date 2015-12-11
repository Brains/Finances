using System;
using System.Collections.ObjectModel;
using System.Linq;
using static Common.Record.Types;
using static Common.Record.Categories;

namespace Common.Storages
{
	public class RandomRecords : IExpenses, IRecordsStorage
	{
		private readonly Random random;

		public ObservableCollection<Record> Records { get; set; }

		public RandomRecords(Random random)
		{
			this.random = random;

			var records = Enumerable.Range(0, 100).Select(index => CreateRandomRecord()).ToList();

			Record[] debts =
			{
				new Record(200, Debt, Maxim, "Out", RandomDay()),
				new Record(200, Debt, Andrey,"Out", RandomDay()),
				new Record(100, Debt, Maxim, "In",	RandomDay()),
				new Record(100, Debt, Andrey,"In",	RandomDay()),
			};

			records.RemoveAll(record => record.Type == Debt);

			Records = new ObservableCollection<Record>(records.Concat(debts));
		}

		public void Add(Record record) => Records.Add(record);

		private Record CreateRandomRecord()
		{
			string[] descriptions =
			{
				"Novus", "Paint", "Препараты", "Computer", "Техника", "Доктор", "Sport"
			};

			return new Record(random.Next(50, 2000),
				RandomEnumValue<Record.Types>(),
				RandomEnumValue<Record.Categories>(),
				descriptions[random.Next(descriptions.Length)],
				RandomDay());
		}


		private T RandomEnumValue<T>()
		{
			return Enum.GetValues(typeof (T))
			           .Cast<T>()
			           .OrderBy(v => random.Next())
			           .First();
		}


		private DateTime RandomDay()
		{
			var start = new DateTime(2015, 11, 1);
			var range = (DateTime.Today - start).Days;

			return start.AddDays(random.Next(range));
		}
	}
}