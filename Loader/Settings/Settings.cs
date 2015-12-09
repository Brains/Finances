using System.Collections.Generic;
using Common;
using static Common.Record;
using static Common.Record.Categories;
using static Common.Record.Types;
using Mapping = System.Collections.Generic.Dictionary<Common.Record.Types, Common.Record.Categories[]>;

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