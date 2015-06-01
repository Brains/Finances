using System;

namespace Statistics.Banking
{
	public interface IFundsStorage
	{
		void Get(Action<decimal> callback);
	}
}