using System.Collections.ObjectModel;

namespace Common
{
	public interface IExpenses
	{
		ObservableCollection<Record> Records { get; set; }
	}
}