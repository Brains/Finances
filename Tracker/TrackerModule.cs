using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Tracker
{
	public class TrackerModule : IModule
	{
		private readonly IUnityContainer container;
		//------------------------------------------------------------------
		public TrackerModule (IUnityContainer container)
		{
			this.container = container;

			
		}

		//------------------------------------------------------------------
		public void Initialize ()
		{
			container.RegisterType<IExpenses, Expenses>();

			var regionManager = container.Resolve<IRegionManager>();

			regionManager.RegisterViewWithRegion("MainRegion", () => this.container.Resolve<IExpenses>());
		}
	}
}
