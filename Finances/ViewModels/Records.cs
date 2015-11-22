using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Caliburn.Micro;

namespace Finances.ViewModels
{
	public class Records : PropertyChangedBase, IViewModel
	{
		public IEnumerable<Record> RecordsList { get; }

		public Records(IObservableCollection<Record> records)
		{
			RecordsList = records;
		}
	}

	public class Record {}
}
