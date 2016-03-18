using System;
using System.ComponentModel;

namespace Common
{
	public interface IFundsSource
	{
		void PullValue();
		event Action<decimal> Update;
	}
}