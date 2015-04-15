using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker;

namespace Visualization.ViewModels
{
	// For XAML
	//------------------------------------------------------------------
	public class MarkupDictionary : Dictionary<string, int> { }

	class Categories
	{
		//------------------------------------------------------------------
		public Dictionary<string, int> Data { get; }

		//------------------------------------------------------------------
		public Categories ()
		{
			Data = new Dictionary<string, int>();

			var expenses = new Expenses().Records;

			foreach (Record.Categories category in Enum.GetValues(typeof(Record.Categories)))
			{
				Data.Add(category.ToString(), Sum(expenses, category));
			}

		}

		//------------------------------------------------------------------
		private static int Sum (List<Record> expenses, Record.Categories type)
		{
			var asd = from record in expenses
					  where record.Category == type
					  select record.Amount;

			return (int) asd.Aggregate((first, second) => first + second);
		}
	}
}
