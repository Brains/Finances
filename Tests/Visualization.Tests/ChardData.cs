using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;
using Tracker;
using Visualization.ViewModels;

namespace Visualization.Tests
{
	[TestFixture]
    public class ChardData : AssertionHelper
    {
		public List<Record> Records { get; set; } = new List<Record>();

		[SetUp]
		//------------------------------------------------------------------
		public void LoadData ()
		{
            XmlSerializer serializer = new XmlSerializer(typeof (List<Record>));

			using (StreamReader stream = new StreamReader("Records.xml"))
			using (var reader = XmlReader.Create(stream))
			{
				Records = (List<Record>) serializer.Deserialize(reader);
			}
		}

		//------------------------------------------------------------------
		[Test]
		public void Test ()
		{
			Charts charts = new Charts(new Expenses());

			var actual = charts.GetDatesData(Records);

			Dictionary<string, int> expected = new Dictionary<string, int> {["Expense"] = 25685, ["Income"] = 28785};
			Expect(actual, EquivalentTo(expected));
		}

    }
}
