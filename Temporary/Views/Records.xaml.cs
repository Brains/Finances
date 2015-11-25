using Temporary.ViewModels;

namespace Temporary.Views
{
    public partial class Records
    {
	    //------------------------------------------------------------------
		public Records()
        {
			InitializeComponent();
        }

	    //------------------------------------------------------------------
	    public Records (ViewModels.Records viewModel) : this()
	    {
		    DataContext = viewModel;
	    }
    }
}
