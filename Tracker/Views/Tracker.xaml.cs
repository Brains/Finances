using System.Windows.Controls;

namespace Tracker.Views
{
    /// <summary>
    /// Interaction logic for Tracker.xaml
    /// </summary>
    public partial class Tracker : UserControl
    {
        public Tracker()
        {
            InitializeComponent();

	        dataGrid.DataContext = new Expenses();
        }
    }
}
