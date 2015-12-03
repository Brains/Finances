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
			ConventionManager.ApplyValueConverter = ApplyValueConverter;
			ConventionManager.ApplyUpdateSourceTrigger = ApplyUpdateSourceTrigger;
		}

		private void ApplyUpdateSourceTrigger(DependencyProperty dependency, DependencyObject element, Binding binding, PropertyInfo property)
		{
			if (dependency == TextBox.TextProperty && typeof (decimal).IsAssignableFrom(property.PropertyType))
			{
				binding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;

				var textBox = (TextBox) element;
				var context = (Form) textBox.DataContext;
				binding.ConverterParameter = context;
			}
		}

		private void ApplyValueConverter(Binding binding, DependencyProperty dependency, PropertyInfo property)
		{
			previous(binding, dependency, property);

			if (dependency == TextBox.TextProperty && typeof (decimal).IsAssignableFrom(property.PropertyType))
			{
				binding.Converter = container.Resolve<IValueConverter>("AmountTextToDecimal");
            }
		}
	}
}