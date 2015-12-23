using System.Linq;
using Caliburn.Micro;

namespace UI.ViewModels
{
	public interface IShell {}

	public class Shell : Conductor<IScreen>.Collection.OneActive, IShell
	{
		public IScreen SelectedItem { get; set; }
		public Shell(IScreen[] screens)
		{
			DisplayName = "Finances";

			Items.AddRange(screens);

			ActivateItem(Items[0]);
        }
	}
}