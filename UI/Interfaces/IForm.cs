using System.Collections;
using System.Collections.Generic;
using Records;

namespace UI.Interfaces
{
	public interface IForm
	{
		int Amount { get; set; }
		void Submit();
	}
}