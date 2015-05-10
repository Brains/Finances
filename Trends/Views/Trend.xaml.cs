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
using NodaTime;

namespace Trends.Views
{
	//------------------------------------------------------------------
	public partial class Trend
	{
		//------------------------------------------------------------------
		public Trend()
		{
			InitializeComponent();
		}

		[InjectionConstructor]
		//------------------------------------------------------------------
		public Trend(ViewModels.Trend viewModel) : this()
	    {
			DataContext = viewModel;
		}
	}

	namespace Design
	{
		public class Trend
		{
			//------------------------------------------------------------------
			public List<Funds> Funds { get; set; }

			//------------------------------------------------------------------
			public Trend()
			{
				Funds = new List<Funds>
				{
					new Funds(5000, new LocalDate(2015, 1, 1), "Food"),
					new Funds(4800, new LocalDate(2015, 1, 5), "Mozy"),
					new Funds(5200, new LocalDate(2015, 1, 5), "O3"),
					new Funds(4100, new LocalDate(2015, 1, 5), "Deposit"),
					new Funds(3600, new LocalDate(2015, 1, 7), "Deposit"),
					new Funds(4800, new LocalDate(2015, 1, 14), "Deposit"),
					new Funds(4400, new LocalDate(2015, 1, 15), "House")
				};

			}
		}
	}

}
