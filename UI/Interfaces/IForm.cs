using System.Collections;
using System.Collections.Generic;
using Records;

namespace UI.Interfaces
{
	public interface IForm
	{
		decimal Amount { get; set; }
		void Submit();
	}
}