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
		string BankRequest { get; set; }
		string Cash { get; set; }
		string RecordsPath { get; set; }
		string[] Descriptions { get; set; }
		int Customers { get; set; }
	    int Precision { get; set; }
	    void Save(string name, decimal value);
	}
}