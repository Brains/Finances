// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Common;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Tracker
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
			container.RegisterType<IExpenses, Expenses>(new ContainerControlledLifetimeManager());

			var regionManager = container.Resolve<IRegionManager>();

			regionManager.RegisterViewWithRegion("Records", () => this.container.Resolve<Views.Records>());
			regionManager.RegisterViewWithRegion("RecordFormsQueue", () => this.container.Resolve<Views.RecordFormsQueue>());
		}
	}
}
