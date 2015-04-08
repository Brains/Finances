using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeContracts;

namespace Tracker
{
    public class Record
    {
        //------------------------------------------------------------------
	    public enum Types
	    {
		    Expense,
		    Income,
		    Balance,
		    Debt,
	    }

		//------------------------------------------------------------------
		public enum Categories
        {
            Food,
            Health,
            Housing,
            Permanent,
            Income,
        }

	    //------------------------------------------------------------------
        public int ID { get; private set; }
        public Types Type { get; private set; }
        public decimal Amount { get; private set; }
        public Categories Categorie { get; private set; }
        public string Description { get; private set; }
        public DateTime Date { get; private set; }

        //------------------------------------------------------------------
        public Record (int id, decimal amount, Types type, Categories categorie, string description, DateTime date)
        {
			Requires.True(amount > 1, "amount > 1");
			Requires.NotNullOrEmpty(description, "Description");
			Requires.True(date > DateTime.Now.AddHours(-1), "date > DateTime.Now");

	        ID = id;
            Type = type;
            Amount = amount;
            Categorie = categorie;
            Description = description;
            Date = date;
        }
    }
}