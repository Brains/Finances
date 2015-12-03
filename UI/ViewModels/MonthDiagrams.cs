using Caliburn.Micro;
using Records;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class MonthDiagrams : PropertyChangedBase, IViewModel
	{
		private IExpences expences;

		public MonthDiagrams(IExpences expences)
		{
			this.expences = expences;
		}
	}
}