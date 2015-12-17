using System;
using Caliburn.Micro;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Trends : Conductor<object>
	{
		public Trends(IViewModel viewModel)
		{
			DisplayName = "\uE1DC Trends";

			ActivateItem(viewModel);
		}
	}
}