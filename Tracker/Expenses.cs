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
	    private string recordsDataPath;

	    public ObservableCollection<Record> Records { get; set; }
        
        public Expenses ()
        {
			recordsDataPath = Path.Combine("C:\\", "Projects", "Finances", "Data", "Records.xml");

			Records = new ObservableCollection<Record>();
//	        Records.CollectionChanged += (s, a) => Save();
		}

	    public Expenses(bool load) : this()
	    {
		    if (load)
			    Load();
	    }

	    public void Add (decimal amount, Record.Types type, Record.Categories category, string description, DateTime date)
		{
			Record record = new Record(amount, type, category, description, date);
			Records.Add (record);
			Save();
		}

	    public void Load ()
		{
			XmlSerializer serializer = new XmlSerializer(Records.GetType());

			using (StreamReader stream = new StreamReader(recordsDataPath))
			using (var writer = XmlReader.Create(stream))
			{
				Records = (ObservableCollection<Record>) serializer.Deserialize(writer);
			}
		}

	    public void Save ()
	    {
		    XmlSerializer serializer = new XmlSerializer(Records.GetType());

		    using (StreamWriter stream = new StreamWriter(recordsDataPath))
		    using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
		    {
			    serializer.Serialize(writer, Records);
		    }
	    }

	    #region Random
	    
	    private void CreateRandomRecord ()
	    {
		    string[] descriptions =
		    {
			    "Novus", "Paint: Грунт", "Paint: Краска", "Препараты", "Computer", "Техника",
			    "Доктор", "Sport: Диск", "Sport: Мазь",
		    };

		    Records.Add(new Record(random.Next(50, 2000),
			    RandomEnumValue<Record.Types>(),
				RandomEnumValue<Record.Categories>(),
			    descriptions[random.Next(descriptions.Length)],
			    RandomDay(random)));
	    }

	    
	    private T RandomEnumValue<T> () 
	    {
		    return Enum.GetValues(typeof (T))
				.Cast<T>()
				.OrderBy(v => random.Next())
				.FirstOrDefault();
	    }

	    
	    DateTime RandomDay (Random random)
	    {
		    DateTime start = new DateTime(2015, 2, 1);
		    int range = (DateTime.Today - start).Days;
		    return start.AddDays(random.Next(range));
	    }

	    #endregion
	}
}