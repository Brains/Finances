using System;
using System.Windows.Markup;

namespace UI.Views.Extensions
{
	public class EnumValuesExtension : MarkupExtension
	{
		private readonly Type enumType;

		public EnumValuesExtension(Type enumType)
		{
			if (enumType == null)
				throw new ArgumentNullException("enumType");
			if (!enumType.IsEnum)
				throw new ArgumentException("Argument enumType must derive from type Enum.");

			this.enumType = enumType;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return Enum.GetValues(enumType);
		}
	}
}