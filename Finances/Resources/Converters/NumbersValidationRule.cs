using System;
using System.Globalization;
using System.Windows.Controls;

namespace Finances.Resources.Converters
{
	public class NumbersValidationRule : ValidationRule
	{
		public override ValidationResult Validate (object value, CultureInfo cultureInfo)
		{
			try
			{
				AmountTextToDecimal.Summarize((string) value);
			}
			catch (Exception e)
			{
				return new ValidationResult(false, e.Message);
			}

			return new ValidationResult(true, null);
		}
	}
}
