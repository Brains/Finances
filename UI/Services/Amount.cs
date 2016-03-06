using Common;
using UI.Views.Converters;

namespace UI.Services
{
	public interface IAmountFactory
	{
		IAmount Create(Record.Types type);
	}

	public interface IAmount
	{
		decimal Value { get; set; }
		decimal Total { get; }
	}

	public class Amount : IAmount
	{
		public virtual decimal Value { get; set; }

		public virtual decimal Total => Value;
		public override string ToString()
		{
			return Value == 0
				       ? string.Empty
				       : Value.ToString("N0");
		}
	}

	public class SharedAmount : Amount
	{
		private readonly int customers;
		private decimal value;

		public SharedAmount(ISettings settings)
		{
			customers = settings.Customers;
		}

		public override decimal Value
		{
			get { return value; }
			set { this.value = value / customers; }
		}

		public override decimal Total => Value * customers;
	}
}