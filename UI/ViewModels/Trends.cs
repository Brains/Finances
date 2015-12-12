using System;
using Caliburn.Micro;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Trends : Conductor<object>
	{
		public Trends(IViewModel viewModel)
		{
			Icon = "\uE111";
			DisplayName = "Trends";

			ActivateItem(viewModel);
		}

		public string Icon { get; set; }
	}
}