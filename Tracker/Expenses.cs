using System.Collections.Generic;
using Microsoft.Practices.Unity;
using CodeContracts;

namespace Tracker
{
    public class Expenses
    {
        //------------------------------------------------------------------
        public List<Record> Records { get; private set; }

        //------------------------------------------------------------------
        public Expenses ()
        {
            Records = new List<Record> ();
        }

        //------------------------------------------------------------------
        public void Add (Record record)
        {
            Requires.NotNull(record, "record");

            Records.Add (record);
        }
    }
}