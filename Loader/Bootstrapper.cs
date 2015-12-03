using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Loader.Factories;
using Microsoft.Practices.Unity;
using Records;
using UI.Interfaces;
using UI.ViewModels;
using UI.Views.Converters;
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
			ConfigureUnity();
			ConfigureCaliburn();
		}

		private void ConfigureUnity()
		{
			container = new UnityContainer();

			container.RegisterType<IWindowManager, WindowManager>(new Singleton());
			container.RegisterType<IEventAggregator, EventAggregator>(new Singleton());

			container.RegisterType<Random>(new Singleton(), new InjectionConstructor());
			container.RegisterType<IExpences, RandomRecords>(new Singleton());
			container.RegisterType<ISettings, Settings.Settings>(new Singleton());
			container.RegisterType<IRecordsStorage, RandomRecords>(new Singleton());

			ConfigureViewModels();
			ConfigureConverters();
		}

		private void ConfigureConverters()
		{
			container.RegisterType<IAdder, Adder>(new Singleton());
			container.RegisterType<IValueConverter, SharedConverter>("AmountConverter", new Singleton(),
				new InjectionConstructor(new ResolvedParameter<AmountConverter>()));
		}

		private void ConfigureViewModels()
		{
			container.RegisterType<IEnumerable<IScreen>, IScreen[]>();
			container.RegisterType<IShell, Shell>(new PerResolve());

			// Tracker
			container.RegisterType<IScreen, Tracker>("Tracker", new InjectionConstructor(
				new ResolvedParameter<IViewModel>("Records"),
				new ResolvedParameter<IViewModel>("FormsQueue")));
			container.RegisterType<IViewModel, UI.ViewModels.Records>("Records");
			container.RegisterType<IViewModel, FormsQueue>("FormsQueue");
			container.RegisterType<IFormFactory, FormFactory>(new Singleton());
			container.RegisterType<IForm, Form>();

			// Statistics
			container.RegisterType<IScreen, Statistics>("Statistics", new InjectionConstructor(
				new ResolvedParameter<IViewModel>("Records")));


		}

		private void ConfigureCaliburn()
		{
			ViewLocator.NameTransformer.AddRule("Model", string.Empty);
			AssemblySource.Instance.Add(Assembly.GetAssembly(typeof (UI.ViewModels.Shell)));

			new ConvertersFactory(container).Configure();
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