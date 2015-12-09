using System.Collections.Generic;
using static Common.Record;

namespace Common
{
	public interface ISettings
	{
		Dictionary<Record.Types, Record.Categories[]> CategoriesMapping { get; set; }
	}
}