using System;
using Caliburn.Micro;

namespace Finances.ViewModels
{
	public class Tracker : Conductor<object>.Collection.AllActive
	{
		public Tracker(IViewModel records, IViewModel formsQuery)
		{
			DisplayName = "Tracker";

			Items.Add(records);
			Items.Add(formsQuery);
		}
	}
}