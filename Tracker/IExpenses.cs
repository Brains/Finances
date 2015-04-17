using System.Collections.Generic;

namespace Tracker
{
	public interface IExpenses
	{
		void Add (int amount, Record.Types type, Record.Categories category, string description);
		List<Record> Records { get; }
	}
}