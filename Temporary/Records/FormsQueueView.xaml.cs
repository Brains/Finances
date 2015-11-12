namespace Temporary.Records
{
	public partial class FormsQueueView
	{
		public FormsQueueView ()
		{
			InitializeComponent();
		}

		public FormsQueueView (FormsQueueViewModel viewModel) : this()
	    {
			DataContext = viewModel;
        }
	}
}
