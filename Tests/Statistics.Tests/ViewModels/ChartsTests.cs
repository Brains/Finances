using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.Prism.PubSubEvents;
using NSubstitute;
using NUnit.Framework;
using Statistics.ViewModels;
using Tracker;

namespace Statistics.Tests.ViewModels
{
	[TestFixture]
    public class ChartsTests : AssertionHelper
    {
		private DateTime date = new DateTime(1, 1, 1);

		private List<Record> LoadData ()
		{
			List<Record> records;
            XmlSerializer serializer = new XmlSerializer(typeof (List<Record>));

			using (StreamReader stream = new StreamReader("Records.xml"))
			using (var reader = XmlReader.Create(stream))
			{
				records = (List<Record>) serializer.Deserialize(reader);
			}

			return records;
		}

//		[Ignore]
//		[Test]
//		public void TestINVALID ()
//		{
//			Charts charts = new Charts(new Expenses());
//
//			var actual = charts.GetDatesData(LoadData());
//
//			Dictionary<string, int> expected = new Dictionary<string, int> {["Expense"] = 25685, ["Income"] = 28785};
//			Expect(actual, EquivalentTo(expected));
//		}

		[Test]
		public void GetRecordsFrom_Date_ReturnsOnlyRecordsAfterThisDate()
		{
			var expenses = Substitute.For<IExpenses>();
			Charts charts = Create(expenses);
			DateTime start = new DateTime(2015, 4, 1);

			var actual = charts.GetRecordsFrom(start, LoadData());

			Expect(actual, Is.All.Property("Date").GreaterThan(start));
		}

		private static Charts Create(IExpenses expenses)
		{
			IEventAggregator aggregator = Substitute.For<IEventAggregator>();

			return new Charts(expenses, aggregator);
		}

		[Test]
		public void MyMethod()
		{
			var expenses = Substitute.For<IExpenses>();
			expenses.Records = new ObservableCollection<Record>
			{
				new Record(100, Record.Types.Expense, Record.Categories.Food, "1", date),
				new Record(100, Record.Types.Shared, Record.Categories.Food, "1", date),
				new Record(100, Record.Types.Income, Record.Categories.Deposit, "1", date)
			};

			Charts charts = Create(expenses);

			var actual = charts.CalculateInOut();

			foreach (var pair in actual)
			{
				Console.WriteLine($"{pair.Key} - {pair.Value}");
			}



			Expect(actual, Count.EqualTo(2));
			Expect(actual, Count.EqualTo(2));
			Expect(actual, Exactly(1).Property("Key").EqualTo(Record.Types.Expense));
			Expect(actual, Exactly(1).Property("Key").EqualTo(Record.Types.Income));
			Expect(actual, Exactly(1).Property("Value").EqualTo(200));
			Expect(actual, Exactly(1).Property("Value").EqualTo(100));
		}



		
    }
}
