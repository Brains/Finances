using System;
using System.Collections.ObjectModel;

namespace Records
{
	public interface IExpenses
	{
		ObservableCollection<Record> Records { get; set; }
		void Add(decimal amount, Record.Types type, Record.Categories category, string description, DateTime date);
	}
}