using System.Collections.Generic;
using Caliburn.Micro;

namespace Finances.ViewModels
{
	public interface IShell {}

	public class Shell : Conductor<IScreen>.Collection.OneActive, IShell
	{
		public Shell(IScreen forms)
		{
			DisplayName = "Finances";

			Items.Add(forms);
		}
	}
}