using System.Threading.Tasks;

namespace UI.Services
{
	public interface IFundsSource
	{
		string Name { get; set; }
		decimal Value { get; set; }
	}
}