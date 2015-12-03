using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Records
{
	public class RandomRecords : IExpences, IRecordsStorage
	{
		private readonly Random random;

		public ObservableCollection<Record> Records { get; set; }

		public RandomRecords(Random random)
		{
			this.random = random;

			var records = Enumerable.Range(0, 30).Select(index => CreateRandomRecord());
			Records = new ObservableCollection<Record>(records);
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