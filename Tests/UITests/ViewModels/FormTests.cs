using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UI.ViewModels;
using static Records.Record;

namespace UITests.ViewModels
{
	public class FormTests : AssertionHelper
	{
		[Test]
		public void Types_Always_ContainsAllMembersOfTypesEnum()
		{
			var form = new Form();

			IEnumerable<Types> expected = new []{Types.Balance, Types.Debt, Types.Expense, Types.Income,  Types.Shared, } ;
			Expect(form.Types, EquivalentTo(expected));
		}
	}
}