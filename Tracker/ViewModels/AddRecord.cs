using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Tracker.ViewModels
{
	class AddRecord
	{
		public string Amount { get; set; }
		public string Description { get; set; }
        public Record.Types Type { get; set; }
		public Record.Categories Category { get; set; }
		public ICommand SubmitCommand { get; private set; }

		//------------------------------------------------------------------
		public AddRecord ()
		{
			SubmitCommand = new DelegateCommand<object>(OnSubmit);
		}

		//------------------------------------------------------------------
		private void OnSubmit (object arg)
		{

		}
	}
}
