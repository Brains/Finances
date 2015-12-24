using System;
using System.Linq;

namespace UI.Views.Converters
{
	public interface IAdder
	{
		decimal Convert(string amount);
	}

	public class Adder : IAdder
	{
		public decimal Convert(string amount)
		{
			if (string.IsNullOrEmpty(amount))
				throw new ArgumentNullException(nameof(amount), "Empty");

			var amounts = amount.Split('+').ToList();
			amounts.RemoveAll(string.IsNullOrWhiteSpace);
			var decimals = amounts.Select(decimal.Parse).ToArray();
			var sum = decimals.Sum();

			if (sum <= 0)
				throw new ArgumentException("Negative", nameof(sum));

			return sum;
		}
	}
}