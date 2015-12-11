using System.Collections.ObjectModel;

namespace Common.Storages
{
	public interface IExpenses
	{
		ObservableCollection<Record> Records { get; set; }
	}
}