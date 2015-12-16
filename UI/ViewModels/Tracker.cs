using Caliburn.Micro;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Tracker : Conductor<object>.Collection.AllActive
	{
		public Tracker(IViewModel[] viewModels)
		{
			Icon = "\uE14C";
			DisplayName = "Tracker";

			Items.AddRange(viewModels);

			Records = viewModels[0];
			Forms = viewModels[1];
		}

		public string Icon { get; set; }
		public IViewModel Records { get; set; }
		public IViewModel Forms { get; set; }
	}
}