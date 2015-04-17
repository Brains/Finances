using System.Windows.Controls;

namespace Tracker.Views
{
    public partial class Records
    {
	    //------------------------------------------------------------------
		public Records()
        {
			InitializeComponent();

			DataContext = new ViewModels.Records();
        }
    }
}
