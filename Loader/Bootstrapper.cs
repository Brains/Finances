using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Loader.Factories;
using Microsoft.Practices.Unity;
using Common;
using Common.Storages;
using Funds;
using Funds.Bank;
using UI.Interfaces;
using UI.Services;
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
			container.RegisterType<ISettings, Settings.Settings>(new Singleton());

			container.RegisterType<IExpenses, FixedRecords>(new Singleton())
			         .RegisterType<IRecordsStorage, FixedRecords>(new Singleton());

			container.RegisterType<IRequestBuilder, RequestBuilder>()
                     .RegisterType<IResponceParser, ResponceParser>(new Singleton())
                     .RegisterType<IEncryption, Encryption>(new Singleton());

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
			container.RegisterType<IShell, Shell>(new PerResolve());

			container.RegisterType<IScreen, Tracker>("Tracker", new InjectionConstructor(
					new ResolvedArrayParameter<IViewModel>(
						new ResolvedParameter<IViewModel>("Records"),
						new ResolvedParameter<IViewModel>("FormsQueue"))))
			         .RegisterType<IViewModel, UI.ViewModels.Records>("Records")
			         .RegisterType<IViewModel, FormsQueue>("FormsQueue")
			         .RegisterType<IFormFactory, FormFactory>(new Singleton())
			         .RegisterType<IForm, Form>();

			container.RegisterType<IScreen, Statistics>("Statistics", new InjectionConstructor(
					new ResolvedArrayParameter<IViewModel>(
						new ResolvedParameter<IViewModel>("Funds"),
						new ResolvedParameter<IViewModel>("Diagrams"))))
			         .RegisterType<IViewModel, Diagrams>("Diagrams")
			         .RegisterType<IViewModel, UI.ViewModels.Funds>("Funds")
			         .RegisterType<IAnalyzer, Analyzer>()
			         .RegisterType<IFundsSource, CardSource>("CardSource");
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

namespace UI.ViewModels
{
}