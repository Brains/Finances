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
        public IForm Primary { get; private set; }

        public void Add(IForm form)
		{
			form.PropertyChanged += OnPropertyChanged;

			Primary = Primary ?? form;
		}

        public void Remove(IForm form)
        {
            if (form == Primary)
                Primary = null;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs arguments)
		{
		    if (arguments.PropertyName != nameof(Form.Amount)) return;
		    if (sender == Primary) return;

		    var form = (IForm) sender;
            Primary.Subtract(form.Amount);
			form.PropertyChanged -= OnPropertyChanged;
        }
    }

    public interface ISubtractor
    {
        void Add(IForm form);
        void Remove(IForm last);
    }
}