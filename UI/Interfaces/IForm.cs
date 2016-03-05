using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using UI.Services;
using static Common.Record;

namespace UI.Interfaces
{
	public interface IForm : INotifyPropertyChanged
	{
		IAmount Amount { get; set; }
		Types SelectedType { get; set; }
		Brush Background { get; set; }
		void Submit();
		bool CanSubmit();
	}
}