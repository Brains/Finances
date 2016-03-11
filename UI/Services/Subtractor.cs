using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UI.Interfaces;
using UI.ViewModels;

namespace UI.Services
{
	public class Subtractor
	{
		private readonly IList<IForm> forms;

		public Subtractor()
		{
			forms = new List<IForm>();
		}

		public void Add(IForm form)
		{
			forms.Add(form);
			form.PropertyChanged += OnPropertyChanged;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs arguments)
		{
			if (arguments.PropertyName != nameof(Form.Amount))
				return;

			forms.First().Subtract(forms.Last());
		}
	}
}