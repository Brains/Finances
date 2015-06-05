using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tracker
{
	public interface IExpenses
	{
		void Add (decimal amount, Record.Types type, Record.Categories category, string description, DateTime date);
		ObservableCollection<Record> Records { get; set; }
		void Load ();
	}
}