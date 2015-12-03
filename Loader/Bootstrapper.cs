using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Loader.Factories;
using Microsoft.Practices.Unity;
using Records;
using UI.Interfaces;
using UI.ViewModels;
using Singleton = Microsoft.Practices.Unity.ContainerControlledLifetimeManager;
using PerResolve = Microsoft.Practices.Unity.PerResolveLifetimeManager;

namespace Loader
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
			ConfigureCaliburn();
			ConfigureUnity();
		}

		private static void ConfigureCaliburn()
		{
			ViewLocator.NameTransformer.AddRule("Model", string.Empty);
			AssemblySource.Instance.Add(Assembly.GetAssembly(typeof (UI.ViewModels.Shell)));
		}

		private void ConfigureUnity()
		{
			container = new UnityContainer();

			container.RegisterType<IWindowManager, WindowManager>(new Singleton());
			container.RegisterType<IEventAggregator, EventAggregator>(new Singleton());
			container.RegisterType<IShell, Shell>(new PerResolve());

			container.RegisterType<Random>(new Singleton(), new InjectionConstructor());
			container.RegisterType<IExpences, RandomRecords>(new Singleton());
			container.RegisterType<ISettings, Settings.Settings>(new Singleton());
			container.RegisterType<IRecordsStorage, RandomRecords>(new Singleton());

			ConfigureViewModels();
		}

		private void ConfigureViewModels()
		{
			container.RegisterType<IViewModel, UI.ViewModels.Records>("Records");
			container.RegisterType<IViewModel, FormsQueue>("FormsQueue");
			container.RegisterType<IScreen, Tracker>(new InjectionConstructor(
				new ResolvedParameter<IViewModel>("Records"),
				new ResolvedParameter<IViewModel>("FormsQueue")));

			container.RegisterType<IFormFactory, FormFactory>(new Singleton());
			container.RegisterType<IForm, Form>();
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