using Caliburn.Micro;
using Temporary.Records;

namespace Temporary.Shell
{
	public class ShellViewModel : PropertyChangedBase, IShell
	{
		public ShellViewModel(FormsQueueViewModel forms)
		{
			Forms = forms;
		}

		public FormsQueueViewModel Forms { get; }
	}
}