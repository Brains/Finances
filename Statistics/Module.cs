// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Statistics.Banking;
using Statistics.ViewModels;

namespace Statistics
{
	public class Module : IModule
	{
		private readonly IUnityContainer container;

		public Module (IUnityContainer container)
		{
			this.container = container;
		}

		public void Initialize ()
		{
			container.RegisterType<IFundsStorage, PrivatBank>("bank");
			container.RegisterType<IFundsStorage, Debts>("debt");


			var regionManager = container.Resolve<IRegionManager>();

			regionManager.RegisterViewWithRegion("Funds", () => this.container.Resolve<Views.Funds>());
			regionManager.RegisterViewWithRegion("Charts", () => this.container.Resolve<Views.Charts>());
		}
	}
}
