using Caliburn.Micro;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Tracker : Conductor<object>.Collection.AllActive
	{
		public Tracker(IViewModel[] viewModels)
		{
			DisplayName = "\uE14C Tracker";

			Items.AddRange(viewModels);

			Records = viewModels[0];
			Forms = viewModels[1];
		}

		public IViewModel Records { get; set; }
		public IViewModel Forms { get; set; }
	}
}