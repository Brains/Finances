using System.Collections.Generic;
using static Records.Record;

namespace Records
{
	public interface ISettings
	{
		Dictionary<Types, Categories[]> CategoriesMapping { get; set; }
	}
}