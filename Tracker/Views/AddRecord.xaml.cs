using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;

namespace Tracker.Views
{
	public partial class AddRecord
	{
		public AddRecord ()
		{
			InitializeComponent();
		}

		[InjectionConstructor]
		//------------------------------------------------------------------
		public AddRecord (ViewModels.AddRecord viewModel) : this()
	    {
			DataContext = viewModel;
			type.ItemsSource = Enum.GetValues(typeof(Record.Types)).Cast<Record.Types>();
			category.ItemsSource = Enum.GetValues(typeof(Record.Categories)).Cast<Record.Categories>();
        }
	}
}
