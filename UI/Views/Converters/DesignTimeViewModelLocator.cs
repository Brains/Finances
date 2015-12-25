using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Caliburn.Micro;
using MoreLinq;

namespace UI.Views.Converters
{
	public class DesignTimeViewModelLocator : IValueConverter
	{
		public static DesignTimeViewModelLocator Instance = new DesignTimeViewModelLocator();

		private static readonly SimpleContainer container;

		static DesignTimeViewModelLocator()
		{
			if (!Execute.InDesignMode) return;

			var type = typeof (DesignTimeViewModelLocator);

			AssemblySource.Instance.Clear();
			AssemblySource.Instance.Add(type.Assembly);

			container = new SimpleContainer();
			IoC.GetInstance = container.GetInstance;
			IoC.GetAllInstances = container.GetAllInstances;
			IoC.BuildUp = container.BuildUp;

			var viewModels = type.Assembly.DefinedTypes
			                     .Where(t => t.IsSubclassOf(typeof (PropertyChangedBase)));

			viewModels.ForEach(vm => container.RegisterPerRequest(vm.AsType(), null, vm.AsType()));

			container.Singleton<IEventAggregator, EventAggregator>();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// Blend creates types from a runtime/dynamic assembly, so match on name/namespace
			var type = typeof (DesignTimeViewModelLocator);
			var viewModelType = type.Assembly.DefinedTypes
			                        .First(t => t.Name == value.GetType().Name &&
			                                    value.GetType().Namespace.EndsWith(t.Namespace))
			                        .AsType();

			return container.GetInstance(viewModelType, null);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}