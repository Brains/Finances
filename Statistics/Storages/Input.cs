using System;
using System.IO;
using System.Xml.Linq;

namespace Statistics.Storages
{
	public class Input : IStorage<decimal>
	{
		private decimal value;
		private readonly FileStorage fileStorage;
		public event Action<decimal> Updated = delegate {};

		public decimal Value
		{
			get { return value; }
			set
			{
				this.value = value;
				Updated(this.value);
			}
		}

		public string Name { get; set; }

		public Input(string name)
		{
			Name = name;

			fileStorage = new FileStorage();
			Value = fileStorage.Load(Name);
		}


		public override string ToString()
		{
			return "ASS";
		}
	}
}