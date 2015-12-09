namespace Common
{
	public interface IFundsSource
	{
		string Name { get; }
		decimal Value { get; set; }
	}
}