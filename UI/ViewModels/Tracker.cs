using Caliburn.Micro;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Tracker : Conductor<object>.Collection.AllActive
	{
		public Tracker(IViewModel[] viewModels)
		{
			Icon = "\uE111";
			DisplayName = "Tracker";

			Items.AddRange(viewModels);
		}

		public string Icon { get; set; }
	}
}