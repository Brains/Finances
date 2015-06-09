using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeContracts;

namespace Tracker
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

		[XmlAttribute(DataType = "dateTime")]
		public DateTime Date { get; set; }

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

			var amount = a.Amount + b.Amount;
			var description = $"{a.Description}\n{b.Description}";

			return new Record(amount, a.Type, a.Category, description, a.Date);
		}
	}
}