using System;
using Caliburn.Micro;
using Temporary.Records;

namespace Temporary.Shell
{
	public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
	{
		public ShellViewModel(IScreen forms)
		{
			Items.Add(forms);
		}
	}
}