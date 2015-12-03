using Caliburn.Micro;
using Records;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class MonthDiagrams : PropertyChangedBase, IViewModel
	{
		private IExpenses expenses;

		public MonthDiagrams(IExpenses expenses)
		{
			this.expenses = expenses;
		}
	}
}