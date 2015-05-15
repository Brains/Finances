// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Trends
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
			var regionManager = container.Resolve<IRegionManager>();

			regionManager.RegisterViewWithRegion("Trends", () => this.container.Resolve<Views.Trend>(new ParameterOverride("startFunds", 5000)));
        }
	}
}
