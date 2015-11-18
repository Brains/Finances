using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Common;
using Microsoft.Practices.Unity;
using Temporary.ViewModels;
using Singleton = Microsoft.Practices.Unity.ContainerControlledLifetimeManager;
using PerResolve = Microsoft.Practices.Unity.PerResolveLifetimeManager;

namespace Temporary
{
	public class Bootstrapper : BootstrapperBase
	{
		private IUnityContainer container;

		public Bootstrapper()
		{
			Initialize();
		}

		protected override void Configure()
		{
			container = new UnityContainer();

			container.RegisterType<IWindowManager, WindowManager>(new Singleton());
			container.RegisterType<IEventAggregator, EventAggregator>(new Singleton());
			container.RegisterType<IShell, ViewModels.Shell>(new PerResolve());
			container.RegisterType<IExpenses, Expenses>(new Singleton());
			container.RegisterType<IScreen, FormsQueue>(new Singleton());

			ViewLocator.NameTransformer.AddRule("Model", string.Empty);

		}

		protected override object GetInstance(Type service, string key)
		{
			var instance = container.Resolve(service, key);
			if (instance != null)
				return instance;

			throw new InvalidOperationException("Could not locate any instances.");
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return container.ResolveAll(service);
		}

		protected override void BuildUp(object instance)
		{
			container.BuildUp(instance);
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<IShell>();
		}
	}
}