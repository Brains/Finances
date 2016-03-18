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
using Funds.Sources;
using MahApps.Metro;
using UI.Interfaces;
using UI.Services;
using UI.Services.Amount;
using UI.ViewModels;
using UI.Views.Converters;
using Singleton = Microsoft.Practices.Unity.ContainerControlledLifetimeManager;
using PerResolve = Microsoft.Practices.Unity.PerResolveLifetimeManager;
using Constructor = Microsoft.Practices.Unity.InjectionConstructor;

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

			container.RegisterType<Random>(new Singleton(), new Constructor());
			container.RegisterType<ISettings, Settings.Settings>(new Singleton());

			container.RegisterType<IExpenses, StoredRecords>(new Singleton())
			         .RegisterType<IRecordsStorage, StoredRecords>(new Singleton());

			container.RegisterType<IRequestBuilder, RequestBuilder>()
                     .RegisterType<IResponceParser, ResponceParser>(new Singleton())
                     .RegisterType<IEncryption, Encryption>(new Singleton());

			ConfigureViewModels();
		}

		private void ConfigureViewModels()
		{
			container.RegisterType<IShell, Shell>(new PerResolve());

			container.RegisterType<IScreen, Tracker>(
				"Tracker", new Constructor(
					           new ResolvedArrayParameter<IViewModel>(
						           new Parameter<IViewModel>("Funds"),
						           new Parameter<IViewModel>("Records"),
						           new Parameter<IViewModel>("FormsQueue"))))
			         .RegisterType<IViewModel, UI.ViewModels.Funds>("Funds")
			         .RegisterType<IViewModel, UI.ViewModels.Records>("Records")
			         .RegisterType<IViewModel, FormsQueue>("FormsQueue")
			         .RegisterType<IFormFactory, FormFactory>(new Singleton())
			         .RegisterType<IAdder, Adder>(new Singleton())
			         .RegisterType<IAmount, Amount>()
			         .RegisterType<IAmount, SharedAmount>("Shared")
			         .RegisterType<IAmountFactory, AmountFactory>()
			         .RegisterType<ISubtractor, Subtractor>()
			         .RegisterType<IForm, Form>();

		    container.RegisterType<IScreen, Statistics>("Statistics", new Constructor(
		        new ResolvedArrayParameter<IViewModel>(
		            new Parameter<IViewModel>("Diagrams"))))
		        .RegisterType<IViewModel, Diagrams>("Diagrams");

		    container.RegisterType<IFundsSource, Card>("Card")
		             .RegisterType<IFundsSource, Cash>("Cash")
		             .RegisterType<IFundsSource, Debts>("Debts");

		    container.RegisterType<IFund, Fund>("Card", new Constructor(new Parameter<IFundsSource>("Card"), false))
		             .RegisterType<IFund, Fund>("Cash", new Constructor(new Parameter<IFundsSource>("Cash"), true))
		             .RegisterType<IFund, Fund>("Debts", new Constructor(new Parameter<IFundsSource>("Debts"), false));

            container.RegisterType<IScreen, Trends>("Trends", new Constructor(
						new Parameter<IViewModel>("Trend")))
			         .RegisterType<IViewModel, Trend>("Trend");
        }

		private void ConfigureCaliburn()
		{
			ViewLocator.NameTransformer.AddRule("Model", string.Empty);
			AssemblySource.Instance.Add(Assembly.GetAssembly(typeof (UI.ViewModels.Shell)));
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
			SetCustomAccentColor();

			DisplayRootViewFor<IShell>();
		}

		private void SetCustomAccentColor()
		{
			var name = "Custom";

			ThemeManager.AddAccent(name, new Uri("pack://application:,,,/UI;component/Views/Resources/Accent.xaml"));

			var theme = ThemeManager.DetectAppStyle(Application.Current);

			ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(name), theme.Item1);
		}

        class Parameter<T> : ResolvedParameter<T>
        {
            public Parameter() { }
            public Parameter(string name) : base(name) { }
        }
    }
}
