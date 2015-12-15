using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static Common.Record;

namespace Common
{
	public interface ISettings
	{
		Dictionary<Types, Categories[]> CategoriesMapping { get; set; }
		string ID { get; set; }
		string Password { get; set; }
		string Card { get; set; }
		string BankRequest { get; }
		string Cash { get; set; }
		PermanentOperation[] PermanentOperations { get; set; }
		string RecordsPath { get; set; }
		string[] Descriptions { get; }
		void Save(string name, decimal value);
	}
}