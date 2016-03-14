using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UI.Interfaces;
using UI.ViewModels;

// TODO: Subtract decimal

namespace UI.Services
{
    public class Subtractor : ISubtractor
    {
	    private IForm primary;

		public void Add(IForm form)
		{
			form.PropertyChanged += OnPropertyChanged;

			primary = primary ?? form;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs arguments)
		{
		    if (arguments.PropertyName != nameof(Form.Amount)) return;
		    if (sender == primary) return;

		    var form = (IForm) sender;
            primary.Subtract(form);
			form.PropertyChanged -= OnPropertyChanged;
        }
    }

    public interface ISubtractor
    {
        void Add(IForm form);
    }
}