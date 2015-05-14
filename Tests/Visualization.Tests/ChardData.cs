using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using NSubstitute;
using NUnit.Framework;
using Tracker;
using Visualization.ViewModels;

namespace Visualization.Tests
{
	[TestFixture]
    public class ChardData : AssertionHelper
    {
		//------------------------------------------------------------------
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

		//------------------------------------------------------------------
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

		//------------------------------------------------------------------
		[Test]
		public void GetRecordsFrom_Date_ReturnsOnlyRecordsAfterThisDate()
		{
			var expenses = Substitute.For<IExpenses>();
			Charts charts = new Charts(expenses);
			DateTime start = new DateTime(2015, 4, 1);

			var actual = charts.GetRecordsFrom(start, LoadData());

			Expect(actual, Is.All.Property("Date").GreaterThan(start));
		}

    }
}
