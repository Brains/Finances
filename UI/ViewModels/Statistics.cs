using Caliburn.Micro;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Statistics : Conductor<object>.Collection.AllActive
	{
		public Statistics(IViewModel[] viewModels)
		{
			Icon = "\uE1E7";
			DisplayName = "Statistics";

			Items.AddRange(viewModels);
		}

		public string Icon { get; set; }
	}
}