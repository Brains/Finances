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

	        Chart.DataContext = new ViewModels.Categories();
        }


    }
}
