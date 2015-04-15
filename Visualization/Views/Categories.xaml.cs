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

//			Chart.DataContext = new KeyValuePair<string, int>[] {
//									new KeyValuePair<string, int>("Dog", 30),
//									new KeyValuePair<string, int>("Cat", 25),
//									new KeyValuePair<string, int>("Rat", 5),
//									new KeyValuePair<string, int>("Hampster", 8),
//									new KeyValuePair<string, int>("Rabbit", 12) };

	        Chart.DataContext = new ViewModels.Categories();
        }


    }
}
