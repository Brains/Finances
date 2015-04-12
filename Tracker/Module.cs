// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace Tracker
{
	public class Module : IModule
	{
		private readonly IRegionViewRegistry regionViewRegistry;

		public Module (IRegionViewRegistry registry)
		{
			this.regionViewRegistry = registry;
		}

		public void Initialize ()
		{
			regionViewRegistry.RegisterViewWithRegion("MainRegion", typeof(global::Tracker.Views.Tracker));
		}
	}
}
