using Caliburn.Micro;

namespace UI.ViewModels
{
	public class Tracker : Conductor<object>.Collection.AllActive
	{
		public Tracker(IViewModel records, IViewModel formsQuery)
		{
			Icon = "\uE111";
			DisplayName = "Tracker";

			Items.Add(records);
			Items.Add(formsQuery);
		}

		public string Icon { get; set; }
	}
}