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

		private const string BankRequest =
			@"<?xml version=""1.0"" encoding=""UTF-8""?>
			<request version=""1.0"">
				<merchant>
					<id>NULL</id>
					<signature>NULL</signature>
				</merchant>
				<data>
					<oper>cmt</oper>
					<wait>0</wait>
					<test>0</test>
					<payment id="""">
						<prop name=""card"" value=""NULL"" />
						<prop name=""country"" value=""UA"" />
						<prop name=""sd"" value=""NULL"" />
						<prop name=""ed"" value=""NULL"" />
					</payment>
				</data>
			</request>";
	}
}