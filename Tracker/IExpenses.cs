using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tracker
{
	public interface IExpenses
	{
		void Add (decimal amount, Record.Types type, Record.Categories category, string description);
		ObservableCollection<Record> Records { get; }
	}
}