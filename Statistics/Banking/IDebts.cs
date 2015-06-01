using System.Collections.Generic;
using Tracker;

namespace Statistics.Banking
{
	public interface IDebts
	{
		Dictionary<Record.Categories, int> Calculate();
	}
}