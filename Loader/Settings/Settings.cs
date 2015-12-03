using System.Collections.Generic;
using Records;
using static Records.Record;
using static Records.Record.Categories;
using static Records.Record.Types;
using Mapping = System.Collections.Generic.Dictionary<Records.Record.Types, Records.Record.Categories[]>;

namespace Loader.Settings
{
	public class Settings : ISettings
	{
		public Mapping CategoriesMapping { get; set; } = new Mapping
		{
			[Expense]	= new[]{Food, Health, Women, House, General, Other},
			[Debt]		= new[]{Maxim, Andrey},
			[Income]	= new[]{Deposit, ODesk, Other},
			[Shared]	= new[]{Food, House, General, Other},
		};
	}
}