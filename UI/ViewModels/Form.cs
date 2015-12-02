using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using UI.Interfaces;
using static Records.Record;

namespace UI.ViewModels
{
	public class Form : PropertyChangedBase, IForm
	{
		public int Amount { get; set; } 
		public IEnumerable<Types> Types { get; set; }
		public IEnumerable<Categories> Categories { get; set; }
		public Types SelectedType { get; set; }
		public Categories SelectedCategory { get; set; }
		public string Description { get; set; }

		public Form()
		{
			Types = Enum.GetValues(typeof (Types)).Cast<Types>();
			Categories = Enum.GetValues(typeof(Categories)).Cast<Categories>();

			SelectedType = Types.First();
			SelectedCategory = Categories.First();
		}

		public void Submit()
		{
			throw new NotImplementedException();
		}
	}
}