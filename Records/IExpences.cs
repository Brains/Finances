using System.Collections.ObjectModel;

namespace Records
{
	public interface IExpences
	{
		ObservableCollection<Record> Records { get; set; }
	}
}