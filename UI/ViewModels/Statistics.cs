using Caliburn.Micro;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Statistics : Conductor<object>.Collection.AllActive
	{
		public Statistics(IViewModel month)
		{
			Icon = "\uE111";
			DisplayName = "Statistics";

			Items.Add(month);
		}

		public string Icon { get; set; }
	}
}