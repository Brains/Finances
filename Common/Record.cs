using System;
using System.Xml.Serialization;

namespace Common
{
	[Serializable]
    public class Record
	{
        //------------------------------------------------------------------
	    public enum Types
	    {
		    Expense,
		    Income,
		    Shared,
		    Debt,
		    Balance,
		}

		public enum Categories
        {
			Food,
			House,
			Health,
			Other,
			General,
			Women,

			ODesk,
			Deposit,

			Maxim,
			Andrey,
        }

		[XmlAttribute]
		public decimal Amount { get; set; }

		[XmlAttribute]
		public Types Type { get; set; }

		[XmlAttribute]
		public Categories Category { get; set; }

		[XmlAttribute]
		public string Description { get; set; }

		[XmlIgnore]
		public DateTime Date { get; set; }

		[XmlAttribute("Date")]
		public string DateFormatted
		{
			get { return Date.ToString("dd.MM.yy HH:mm:ss"); }
			set { Date = DateTime.Parse(value); }
		}

		public Record () {}
		
        public Record (decimal amount, Types type, Categories category, string description, DateTime date)
        {
            Type = type;
            Amount = amount;
            Category = category;
            Description = description;
            Date = date;
        }

		public override string ToString()
		{
			return $"{Amount}; {Type}; {Category}; {Description}; {Date.ToString("M")}";
		}

		public static Record operator +(Record a, Record b)
		{
			CodeContracts.Requires.True(a.Category == b.Category, "a.Category == b.Category");
			CodeContracts.Requires.True(a.Date.Day == b.Date.Day, "a.Date.Day == b.Date.Day");

			return new Record(a.Amount + b.Amount, a.Type, a.Category, GetAggregatedDescription(a, b), a.Date);
		}

		private static string GetAggregatedDescription(Record a, Record b)
		{
			bool aggregated = false;

			if (a.Description != null)
				aggregated = a.Description.Contains("\n");

			string first = $"{a.Description}";
			if (!aggregated) 
				first += $": {a.Amount}";

			string second = $"{b.Description}: {b.Amount}";

			return $"{first}\n{second}";
		}
	}
}