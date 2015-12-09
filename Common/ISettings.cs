using System.Collections.Generic;
using static Common.Record;

namespace Common
{
	public interface ISettings
	{
		Dictionary<Types, Categories[]> CategoriesMapping { get; set; }
	}
}