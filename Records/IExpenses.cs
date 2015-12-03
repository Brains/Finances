using System.Collections.ObjectModel;

namespace Records
{
	public interface IExpenses
	{
		ObservableCollection<Record> Records { get; set; }
	}
}