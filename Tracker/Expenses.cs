using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.Unity;
using CodeContracts;

namespace Tracker
{
    public class Expenses : IExpenses
    {
		readonly Random random = new Random();

		//------------------------------------------------------------------
		public ObservableCollection<Record> Records { get; private set; }

        //------------------------------------------------------------------
        public Expenses ()
        {
            Records = new ObservableCollection<Record>();

			Load();
		}

		//------------------------------------------------------------------

	    //------------------------------------------------------------------
		public void Add (int amount, Record.Types type, Record.Categories category, string description)
		{
			Record record = new Record(random.Next(1000), amount, type, category, description, DateTime.Now);
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

		//------------------------------------------------------------------
		public void Load ()
		{
			XmlSerializer serializer = new XmlSerializer(Records.GetType());

			using (StreamReader stream = new StreamReader("Records.xml"))
			using (var writer = XmlReader.Create(stream))
			{
				Records = (ObservableCollection<Record>) serializer.Deserialize(writer);
			}
		}



		#region Random
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
			    random.Next(50, 2000),
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
		    DateTime start = new DateTime(2015, 2, 1);
		    int range = (DateTime.Today - start).Days;
		    return start.AddDays(random.Next(range));
	    }

	    #endregion
	}
}