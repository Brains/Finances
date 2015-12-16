using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using static Common.Record;

namespace UI.Interfaces
{
	public interface IForm
	{
		decimal Amount { get; set; }
		Types SelectedType { get; set; }
		Brush Background { get; set; }
		void Submit();
	}
}