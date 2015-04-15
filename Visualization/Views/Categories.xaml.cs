using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Tracker;

namespace Visualization.Views
{
    public partial class Categories : UserControl
    {
	    //------------------------------------------------------------------
        public Categories()
        {
            InitializeComponent();

			Chart.DataContext = new KeyValuePair<string, int>[] {
									new KeyValuePair<string, int>("Dog", 30),
									new KeyValuePair<string, int>("Cat", 25),
									new KeyValuePair<string, int>("Rat", 5),
									new KeyValuePair<string, int>("Hampster", 8),
									new KeyValuePair<string, int>("Rabbit", 12) };


			var expenses = new Expenses().Records;


			var sums = new Dictionary<string, int>();

	        foreach (Record.Categories category in Enum.GetValues(typeof (Record.Categories)))
	        {
				sums.Add(category.ToString(), Sum(expenses, category));

			}





			Chart.DataContext = sums;
        }

	    //------------------------------------------------------------------
	    private static int Sum (List<Record> expenses, Record.Categories type)
	    {
		    var asd = from record in expenses
			    where record.Category == type
			    select record.Amount;

		    return (int) asd.Aggregate((first, second) => first + second);
	    }
    }
}
