using System;
using System.Collections.ObjectModel;

namespace Records
{
	public interface IExpenses
	{
		void Add (decimal amount, Record.Types type, Record.Categories category, string description, DateTime date);
		ObservableCollection<Record> Records { get; set; }
		void Load ();
	}
}