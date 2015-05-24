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

			Max,
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
    }
}