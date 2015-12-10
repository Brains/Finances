using System.Collections.Generic;
using static Common.Record;

namespace Common
{
	public interface ISettings
	{
		Dictionary<Types, Categories[]> CategoriesMapping { get; set; }
		string ID { get; set; }
		string Password { get; set; }
		string Card { get; set; }
	}
}