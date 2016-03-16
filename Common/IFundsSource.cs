using System;
using System.ComponentModel;

namespace Common
{
	public interface IFundsSource
	{
		string Name { get; }
		decimal Value { get; set; }
		void PullValue();
		event Action Update;
	}
}