using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UI.Interfaces;
using UI.ViewModels;

// TODO: Eliminate forms
// TODO: Subtract decimal

namespace UI.Services
{
	public class Subtractor
	{
		private readonly IList<IForm> forms;
		private IForm primary;

		public Subtractor()
		{
			forms = new List<IForm>();
		}

		public void Add(IForm form)
		{
			forms.Add(form);
			form.PropertyChanged += OnPropertyChanged;

			primary = primary ?? form;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs arguments)
		{
			if (sender == primary) return;
			if (arguments.PropertyName != nameof(Form.Amount)) return;

		    var form = (IForm) sender;
            primary.Subtract(form);
			form.PropertyChanged -= OnPropertyChanged;
        }
    }
}