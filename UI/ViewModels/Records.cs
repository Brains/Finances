using System.Collections.Generic;
using Caliburn.Micro;
using Common;
using Common.Storages;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Records : PropertyChangedBase, IViewModel
	{
		public IEnumerable<Record> RecordsList { get; }

		public Records(IExpenses expenses)
		{
			RecordsList = expenses.Records;
		}
	}
}
