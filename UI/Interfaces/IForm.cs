using System.Collections;
using System.Collections.Generic;
using static Common.Record;

namespace UI.Interfaces
{
	public interface IForm
	{
		decimal Amount { get; set; }
		Types SelectedType { get; set; }
		void Submit();
	}
}