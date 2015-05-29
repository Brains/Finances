using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.ObjectBuilder2;
using NSubstitute;
using NUnit.Framework;
using Tracker;
using Visualization.ViewModels;
using static System.Console;
using static Tracker.Record;
using static Tracker.Record.Types;
using static Tracker.Record.Categories;

namespace Visualization.Tests
{
	[TestFixture]
    public class ChardData : AssertionHelper
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
			Charts charts = new Charts(expenses);
			DateTime start = new DateTime(2015, 4, 1);

			var actual = charts.GetRecordsFrom(start, LoadData());

			Expect(actual, Is.All.Property("Date").GreaterThan(start));
		}

		[Test]
		public void MyMethod()
		{
			var expenses = Substitute.For<IExpenses>();
			expenses.Records = new ObservableCollection<Record>
			{
				new Record(100, Expense, Food, "1", date),
				new Record(100, Shared, Food, "1", date),
				new Record(100, Income, Deposit, "1", date)
			};

			Charts charts = new Charts(expenses);

			var actual = charts.GetInOutRatio(expenses.Records);

			foreach (var pair in actual)
			{
				WriteLine($"{pair.Key} - {pair.Value}");
			}



			Expect(actual, Count.EqualTo(2));
			Expect(actual, Count.EqualTo(2));
			Expect(actual, Exactly(1).Property("Key").EqualTo(Expense));
			Expect(actual, Exactly(1).Property("Key").EqualTo(Income));
			Expect(actual, Exactly(1).Property("Value").EqualTo(200));
			Expect(actual, Exactly(1).Property("Value").EqualTo(100));
		}

		[Test]
		public void CalculateDebts_Always_ReturnsTotalDebtForEachDude()
		{
			var expenses = Substitute.For<IExpenses>();
			Charts charts = new Charts(expenses);
			FillDebts(expenses);

			var actual = charts.CalculateDebts();

			var expected = new Dictionary<Categories, int>();
			expected[Max] = 300;
			expected[Andrey] = 300;
			Expect(actual, EquivalentTo(expected));
		}

		private void FillDebts(IExpenses expenses)
		{
			expenses.Records = new ObservableCollection<Record>
			{
				new Record(500, Debt, Max, "Out", date),
				new Record(200, Debt, Andrey, "Out", date),
				new Record(100, Debt, Max, "2", date),
				new Record(100, Debt, Max, "Out", date),
				new Record(200, Debt, Andrey, "2", date),
				new Record(250, Debt, Max, "Out", date),
				new Record(250, Debt, Max, "2", date)
			};
		}
    }
}
