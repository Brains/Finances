using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Statistics.Storages
{
	public class FileStorage
	{
		private string fileName = "Funds.xml";
		private string path;

		public FileStorage()
		{
			path = Path.Combine("Data", fileName);

//			CreateDocument(path);
        }

		public decimal Load(string name)
		{
			if (!File.Exists(path)) return default(decimal);

			return decimal.Parse(XElement.Load(path).Element(name).Value);
		}

		public void Save(string name, decimal value)
		{
			var document = XElement.Load(path);
			document.Element(name).SetValue(value);
			document.Save(path);
		}

		private void CreateDocument(string path)
		{
			XElement document = new XElement("data",
				new XElement("Upwork", 0),
				new XElement("Cash", 0));

			document.Save(path);
		}
	}
}