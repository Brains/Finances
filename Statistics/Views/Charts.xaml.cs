using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Tracker;

namespace Statistics.Views
{
    public partial class Charts : UserControl
    {
	    //------------------------------------------------------------------
        public Charts()
        {
			InitializeComponent();
        }

		[InjectionConstructor]
	    //------------------------------------------------------------------
	    public Charts (ViewModels.Charts viewModel) : this()
	    {
		    DataContext = viewModel;
	    }
	}
}
