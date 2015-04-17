using System.Globalization;
using System.Windows.Controls;

namespace Tracker.Views.Converters
{
	public class NumbersValidationRule : ValidationRule
	{
		public override ValidationResult Validate (object value, CultureInfo cultureInfo)
		{
			string number = value?.ToString();

			int integer;
			bool result = int.TryParse(number, out integer);

			if (!result)
				return new ValidationResult(false, "CHEATER!!!");

			return new ValidationResult(true, null);

		}
	}
}
