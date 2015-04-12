// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Tracker
{
	public class Module : IModule
	{
		private readonly IUnityContainer container;
		private readonly IRegionViewRegistry regionViewRegistry;

		//------------------------------------------------------------------
		public Module (IUnityContainer container)
		{
			this.container = container;
		}

		//------------------------------------------------------------------
		public void Initialize ()
		{
			container.RegisterType<IExpenses, Expenses>();

			var regionManager = container.Resolve<IRegionManager>();

			regionManager.RegisterViewWithRegion("MainRegion", () => this.container.Resolve<Views.Tracker>());
		}
	}
}
