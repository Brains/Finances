using Common;

namespace UI.Services.Amount
{
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