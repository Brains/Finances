using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Finances.ViewModels;
using Microsoft.Practices.Unity;
using Records;
using Singleton = Microsoft.Practices.Unity.ContainerControlledLifetimeManager;
using PerResolve = Microsoft.Practices.Unity.PerResolveLifetimeManager;

namespace Finances
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
			ViewLocator.NameTransformer.AddRule("Model", string.Empty);

			container = new UnityContainer();

			container.RegisterType<IWindowManager, WindowManager>(new Singleton());
			container.RegisterType<IEventAggregator, EventAggregator>(new Singleton());
			container.RegisterType<IShell, Shell>(new PerResolve());

			container.RegisterType<Random>(new Singleton(), new InjectionConstructor());
			container.RegisterType<IExpenses, RandomExpenses>(new Singleton());

			container.RegisterType<IViewModel, ViewModels.Records>("Records");
			container.RegisterType<IViewModel, FormsQueue>("FormsQueue");
			container.RegisterType<IScreen, Tracker>(new InjectionConstructor(
				new ResolvedParameter<IViewModel>("Records"), 
				new ResolvedParameter<IViewModel>("FormsQueue")));


			
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