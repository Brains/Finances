using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker
{
    public class Record
    {
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
        public enum Types
        {
            Income,
            Cost,
            Balance,
        }

        //------------------------------------------------------------------
        private int ID;
        private decimal amount;
        private Categories categorie;
        private string description;
        private DateTime date;
    }
}
