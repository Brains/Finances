using System.Collections.Generic;
using Caliburn.Micro;

namespace Temporary.ViewModels
{
	public class Shell : Conductor<IScreen>.Collection.OneActive, IShell
	{
		public Shell(IScreen forms)
		{
			DisplayName = "Finances";

			Items.Add(forms);
		}
	}
}