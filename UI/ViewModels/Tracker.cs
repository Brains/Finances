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
		}
	}
}