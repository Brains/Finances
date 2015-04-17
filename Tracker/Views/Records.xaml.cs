using System.Windows.Controls;
using Microsoft.Practices.Unity;

namespace Tracker.Views
{
    public partial class Records
    {
	    //------------------------------------------------------------------
		public Records()
        {
			InitializeComponent();
        }

		[InjectionConstructor]
	    //------------------------------------------------------------------
	    public Records (ViewModels.Records viewModel) : this()
	    {
		    DataContext = viewModel;
	    }
    }
}
