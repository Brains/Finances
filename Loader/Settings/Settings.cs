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
		public string ID			{ get; set; } = Config["ID"];
		public string Password		{ get; set; } = Config["Password"];
		public string Card			{ get; set; } = Config["Card"];
		public string Cash			{ get; set; } = Config["Cash"];
		public string RecordsPath	{ get; set; } = Config["RecordsPath"];
		public string[] Descriptions{ get; set; } = Config["Descriptions"].Replace(" ", string.Empty).Split(',');
		public int Customers		{ get; set; } = int.Parse(Config["Customers"]);
        public int Precision        { get; set; } = int.Parse(Config["Precision"]);
	    public int HistoryInterval  { get; set; } = int.Parse(Config["HistoryInterval"]);
        #endregion

        public Mapping CategoriesMapping { get; set; } = new Mapping
		{
			[Expense] = new[] {Food, Health, Women, House, General, Other},
			[Debt] =	new[] {Maxim, Andrey},
			[Income] =	new[] {Deposit, ODesk, Other},
			[Shared] =	new[] {Food, House, General, Other}
		};

		public string BankRequest { get; set; } = 
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