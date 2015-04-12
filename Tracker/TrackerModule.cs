using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Tracker
{
	class TrackerModule : IModule
	{
		private readonly IUnityContainer container;
		//------------------------------------------------------------------
		public TrackerModule (IUnityContainer container)
		{
			this.container = container;

			container.RegisterType<>();
		}

		//------------------------------------------------------------------
		public void Initialize ()
		{
			throw new NotImplementedException();
		}
	}
}
