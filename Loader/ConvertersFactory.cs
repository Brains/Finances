using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Microsoft.Practices.Unity;

namespace Loader
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
		}

		private void ApplyValueConverter(Binding binding, DependencyProperty bindableProperty, PropertyInfo property)
		{
			previous(binding, bindableProperty, property);

			if (bindableProperty == TextBlock.TextProperty && typeof (DateTime).IsAssignableFrom(property.PropertyType))
				binding.Converter = container.Resolve<IValueConverter>("AmountTextToDecimal");
		}
	}
}