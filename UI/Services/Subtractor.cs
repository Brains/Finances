using System.Collections.Generic;
using System.Linq;
using UI.Interfaces;

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
			form.PropertyChanged += (sender, args) => forms.First().Subtract(forms.Last());
		}
	}
}