using Records;

namespace UI.Interfaces
{
	public interface IForm
	{
		int Amount { get; set; }
		Record.Types Type { get; set; }
		Record.Categories Category { get; set; }
		string Description { get; set; }
		void Submit();
	}
}