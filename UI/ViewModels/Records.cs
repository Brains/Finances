using System.Collections.Generic;
using Caliburn.Micro;
using Common;
using Common.Storages;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Records : PropertyChangedBase, IViewModel
	{
		public IEnumerable<Record> List { get; }
		public int RowIndex { get; } = 0;

		public Records(IExpenses expenses)
		{
			List = expenses.Records;
		}
	}
}
