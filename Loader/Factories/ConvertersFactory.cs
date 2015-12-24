using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Microsoft.Practices.Unity;
using UI.ViewModels;

namespace Loader.Factories
{
	public class ConvertersFactory
	{
		private readonly IUnityContainer container;
		private readonly Action<Binding, DependencyProperty, PropertyInfo> previous;

		public ConvertersFactory(IUnityContainer container)
		{
			this.container = container;

			previous = ConventionManager.ApplyValueConverter;
		}

		public void Configure()
		{
			// Use ApplyUpdateSourceTrigger because it has DependencyObject parameter
			ConventionManager.ApplyUpdateSourceTrigger = ConfigureBinding;
		}

		private void ConfigureBinding(DependencyProperty dependency, DependencyObject element, Binding binding, PropertyInfo property)
		{
			if (dependency == TextBox.TextProperty 
				&& typeof (decimal).IsAssignableFrom(property.PropertyType)
				&& property.Name == "Amount")
			{
				var textBox = (TextBox) element;
				binding.ConverterParameter = (Form) textBox.DataContext;
				binding.Converter = container.Resolve<IValueConverter>("AmountSummarizing");
				binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			}
		}
	}
}