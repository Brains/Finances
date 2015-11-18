namespace Temporary.Records
{
    public partial class RecordsView
    {
	    //------------------------------------------------------------------
		public RecordsView()
        {
			InitializeComponent();
        }

	    //------------------------------------------------------------------
	    public RecordsView (RecordsViewModel viewModel) : this()
	    {
		    DataContext = viewModel;
	    }
    }
}
