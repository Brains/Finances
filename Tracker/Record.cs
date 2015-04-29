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

		//------------------------------------------------------------------
		public enum Categories
        {
			Food,
			Domestic,
			Health,
			Other,
			General,
			Women,

			ODesk,
			Deposit,

			Max,
        }

		//------------------------------------------------------------------
		[XmlAttribute]
		public int ID { get; set; }

		[XmlAttribute]
		public decimal Amount { get; set; }

		[XmlAttribute]
		public Types Type { get; set; }

		[XmlAttribute]
		public Categories Category { get; set; }

		[XmlAttribute]
		public string Description { get; set; }

		[XmlAttribute(DataType = "date")]
		public DateTime Date { get; set; }

		//------------------------------------------------------------------
		public Record () {}
		
		//------------------------------------------------------------------
        public Record (int id, decimal amount, Types type, Categories category, string description, DateTime date) /*: this()*/
        {
			Requires.True(amount > 1, "amount > 1");
			Requires.NotNullOrEmpty(description, "Description");
//			Requires.True(date > DateTime.Now.AddHours(-1), "date > DateTime.Now");

	        ID = id;
            Type = type;
            Amount = amount;
            Category = category;
            Description = description;
            Date = date;
        }
    }
}