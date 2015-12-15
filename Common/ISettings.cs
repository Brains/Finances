using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static Common.Record;

namespace Common
{
	public interface ISettings
	{
		Dictionary<Types, Categories[]> CategoriesMapping { get; }
		string ID { get; }
		string Password { get; }
		string Card { get; }
		string BankRequest { get; }
		string Cash { get; }
		PermanentOperation[] PermanentOperations { get; }
		string RecordsPath { get; }
		string[] Descriptions { get; }
		void Save(string name, decimal value);
	}
}