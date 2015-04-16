using System.Windows.Controls;

namespace Tracker.Views
{
    public partial class Tracker
    {
	    //------------------------------------------------------------------
		public Tracker()
        {
			InitializeComponent();

			DataContext = new ViewModels.Tracker();
        }
    }
}
