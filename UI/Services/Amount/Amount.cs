using Common;
using UI.Views.Converters;

namespace UI.Services.Amount
{
	public class Amount : IAmount
	{
		private readonly IAdder adder;

		public Amount(IAdder adder)
		{
			this.adder = adder;
		}

		public string Formatted
		{
			get { return Value == 0 ? string.Empty : Value.ToString("N0"); }
			set { Value = adder.Convert(value); }
		}

		public virtual decimal Value { get; set; }

		public virtual decimal Total => Value;
	}

	public class SharedAmount : Amount
	{
		private readonly int customers;
		private decimal value;

		public SharedAmount(IAdder adder, ISettings settings) : base(adder)
		{
			customers = settings.Customers;
		}

		public override decimal Value
		{
			get { return value; }
			set { this.value = value/customers; }
		}

		public override decimal Total => Value*customers;
	}

	public interface IAmount
	{
		decimal Value { get; set; }
		decimal Total { get; }
	}

	public interface IAmountFactory
	{
		IAmount Create(Record.Types type);
	}
}