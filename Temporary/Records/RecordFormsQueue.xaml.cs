namespace Temporary.Records
{
	public partial class RecordFormsQueue
	{
		public RecordFormsQueue ()
		{
			InitializeComponent();
		}

		[InjectionConstructor]
		public RecordFormsQueue (ViewModels.RecordFormsQueue viewModel) : this()
	    {
			DataContext = viewModel;
        }
	}
}
