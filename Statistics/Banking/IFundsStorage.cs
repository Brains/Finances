using System;

namespace Statistics.Banking
{
	public interface IFundsStorage
	{
		void GetAsync(Action<decimal> callback);
	}
}