using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using Common;
using static System.TimeSpan;
using static Common.Record.Categories;
using static Common.Record.Types;
using Mapping = System.Collections.Generic.Dictionary<Common.Record.Types, Common.Record.Categories[]>;

namespace Loader.Settings
{
	public class Settings : ISettings
	{
		private static readonly NameValueCollection Config;
		private const double Month = 30.436875;

		static Settings()
		{
			Config = ConfigurationManager.AppSettings;
		}

		#region Config
		public string ID			{ get; } = Config["ID"];
		public string Password		{ get; } = Config["Password"];
		public string Card			{ get; } = Config["Card"];
		public string Cash			{ get; } = Config["Cash"];
		public string RecordsPath	{ get; } = Config["RecordsPath"];
		public string[] Descriptions { get; } = Config["Descriptions"].Replace(" ", string.Empty).Split(',');
		#endregion

		public Mapping CategoriesMapping { get; } = new Mapping
		{
			[Expense] = new[] {Food, Health, Women, House, General, Other},
			[Debt] =	new[] {Maxim, Andrey},
			[Income] =	new[] {Deposit, ODesk, Other},
			[Shared] =	new[] {Food, House, General, Other}
		};

		public PermanentOperation[] PermanentOperations { get; } = 
		{
			new PermanentOperation(-2000,	new DateTime(2015, 1, 15),	FromDays(Month),	"House"),
			new PermanentOperation(2000,	new DateTime(2015, 1, 8),	FromDays(Month)	,	"Deposit"),
			new PermanentOperation(-1000,	new DateTime(2015, 5, 30),	FromDays(20),		"Medications"),
			new PermanentOperation(1000,	new DateTime(2015, 1, 17),	FromDays(Month),	"Deposit"),
			new PermanentOperation(-300,	new DateTime(2015, 5, 15),	FromDays(3),		"Food"),
			new PermanentOperation(-500,	new DateTime(2015, 5, 1),	FromDays(7),		"Correction"),
			new PermanentOperation(200,		new DateTime(2015, 1, 7),	FromDays(Month),	"Deposit")
		};

		public string BankRequest { get; } = 
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

		public void Save(string name, decimal value)
		{
			var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
			Configuration config = ConfigurationManager.OpenExeConfiguration(path);

			config.AppSettings.Settings[name].Value = value.ToString(CultureInfo.InvariantCulture);

			// Remove Secured settings
			config.AppSettings.Settings.Remove("ID");
			config.AppSettings.Settings.Remove("Password");
			config.AppSettings.Settings.Remove("Card");

			config.Save(ConfigurationSaveMode.Minimal);
		}
	}
}