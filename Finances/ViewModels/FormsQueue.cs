using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Microsoft.Practices.ObjectBuilder2;

namespace Finances.ViewModels
{
	public class FormsQueue : Screen, IViewModel
	{
<<<<<<< HEAD
=======
		public List<Form> Forms { get; }

		public void Add()
		{
			
		}

		public void Remove()
		{

		}

		public void Submit()
		{

		}

		public bool CanAdd() => false;
		public bool CanRemove() => false;
		public bool CanSubmit() => false;
>>>>>>> Add Guard methods
	}
}