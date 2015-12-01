using System;
using Records;
using UI.Interfaces;

namespace UI.ViewModels
{
	public class Form : IForm
	{
		public int Amount { get; set; }
		public Record.Types Type { get; set; }
		public Record.Categories Category { get; set; }
		public string Description { get; set; }

		public void Submit()
		{
			throw new NotImplementedException();
		}
	}
}