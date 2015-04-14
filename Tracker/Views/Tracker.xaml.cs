using System.Windows.Controls;

namespace Tracker.Views
{
    public partial class Tracker
    {
	    //------------------------------------------------------------------
		public Tracker()
        {
			Resources.Add("Tracker", new ViewModels.Tracker());

			InitializeComponent();
        }
    }
}
