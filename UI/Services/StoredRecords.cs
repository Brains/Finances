using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Common;
using Common.Storages;

namespace UI.Services
{
	public class StoredRecords : IExpenses, IRecordsStorage
	{
		private readonly ISettings settings;

		public StoredRecords(ISettings settings)
		{
			this.settings = settings;

			Records = Load();
		}

		public ObservableCollection<Record> Records { get; set; }

		public void Add(Record record)
		{
			Records.Add(record);
			Save(Records);
		}

		public ObservableCollection<Record> Load()
		{
			var serializer = new XmlSerializer(typeof (ObservableCollection<Record>));
			object records;

			using (var stream = new StreamReader(settings.RecordsPath))
			using (var writer = XmlReader.Create(stream))
			{
				records = serializer.Deserialize(writer);
			}

			return (ObservableCollection<Record>) records;
		}

		public void Save(ObservableCollection<Record> records)
		{
			var serializer = new XmlSerializer(records.GetType());

			using (var stream = new StreamWriter(settings.RecordsPath))
			using (var writer = XmlWriter.Create(stream, new XmlWriterSettings {Indent = true}))
			{
				serializer.Serialize(writer, records);
			}
		}
	}
}