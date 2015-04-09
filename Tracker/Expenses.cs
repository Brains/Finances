using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.Unity;
using CodeContracts;

namespace Tracker
{
    public class Expenses
    {
        //------------------------------------------------------------------
        public List<Record> Records { get; private set; }

        //------------------------------------------------------------------
        public Expenses ()
        {
            Records = new List<Record> ();

	        foreach (var index in Enumerable.Range(0, 100))
	        {
				CreateRandomRecord();
			}

		}

		//------------------------------------------------------------------

	    //------------------------------------------------------------------
		public void Add (Record record)
        {
            Requires.NotNull(record, "record");

            Records.Add (record);
        }

	    //------------------------------------------------------------------
	    public void Save ()
	    {
			XmlSerializer serializer = new XmlSerializer(Records.GetType());

			using (StreamWriter stream = new StreamWriter("Records.xml"))
			using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
			{
				serializer.Serialize(writer, Records);
			}
		}

	    #region Random

	    readonly Random random = new Random();

	    //------------------------------------------------------------------
	    private void CreateRandomRecord ()
	    {
		    string[] descriptions =
		    {
			    "Novus", "Paint: Грунт", "Paint: Краска", "Препараты", "Computer", "Техника",
			    "Доктор", "Sport: Диск", "Sport: Мазь",
		    };

		    Records.Add(new Record(
			    random.Next(1000),
			    random.Next(50, 700),
			    RandomEnumValue<Record.Types>(),
				RandomEnumValue<Record.Categories>(),
			    descriptions[random.Next(descriptions.Length)],
			    RandomDay(random)));
	    }

	    //------------------------------------------------------------------
	    private T RandomEnumValue<T> () 
	    {
		    return Enum.GetValues(typeof (T))
				.Cast<T>()
				.OrderBy(v => random.Next())
				.FirstOrDefault();
	    }

	    //------------------------------------------------------------------
	    DateTime RandomDay (Random random)
	    {
		    DateTime start = new DateTime(2014, 1, 1);
		    int range = (DateTime.Today - start).Days;
		    return start.AddDays(random.Next(range));
	    }

	    #endregion
    }
}