﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Tracker.ViewModels
{
	public class AddRecord
	{
		// Model
		private readonly IExpenses expenses;

		// Record Fields
		public string Amount { get; set; }
		public string Description { get; set; }
        public Record.Types Type { get; set; }
		public Record.Categories Category { get; set; }
		public ICommand SubmitCommand { get; private set; }

		//------------------------------------------------------------------
		public AddRecord (IExpenses expenses)
		{
			this.expenses = expenses;
			SubmitCommand = new DelegateCommand<object>(OnSubmit);
		}

		//------------------------------------------------------------------
		public void OnSubmit (object arg)
		{
			if (string.IsNullOrEmpty(Amount) || string.IsNullOrEmpty(Description))
				return;

			decimal amount = decimal.Parse(Amount);

			if (Type == Record.Types.Shared)
				DivideShared(ref amount);

			expenses.Add(amount, Type, Category, Description);
		}

		//------------------------------------------------------------------
		private decimal DivideShared(ref decimal amount)
		{
			decimal customers = 3;
			amount = Math.Round(amount / customers);

			return amount;
		}
	}
}
