using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Microsoft.Practices.ObjectBuilder2;

namespace Finances.ViewModels
{
	public class FormsQueue : PropertyChangedBase, IViewModel
	{
<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> Add Buttons
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

<<<<<<< HEAD
		public bool CanAdd() => false;
		public bool CanRemove() => false;
		public bool CanSubmit() => false;
>>>>>>> Add Guard methods
=======
>>>>>>> Add Buttons
	}

	public class Form { }
}