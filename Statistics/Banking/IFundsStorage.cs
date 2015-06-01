using System;

namespace Visualization.Banking
{
	public interface IFundsStorage
	{
		void GetAsync(Action<decimal> callback);
	}
}