using Caliburn.Micro;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Statistics : Conductor<object>.Collection.AllActive
	{
		public Statistics(IViewModel[] viewModels)
		{
			DisplayName = "\uE1E7 Statistics";

			Items.AddRange(viewModels);
		}
	}
}