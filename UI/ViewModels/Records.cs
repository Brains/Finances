using System.Collections.Generic;
using Caliburn.Micro;
using Records;

namespace UI.ViewModels
{
	public class Records : PropertyChangedBase, IViewModel
	{
		public IEnumerable<Record> RecordsList { get; }

		public Records(IExpences expences)
		{
			RecordsList = expences.Records;
		}
	}
}
