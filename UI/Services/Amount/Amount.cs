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
}