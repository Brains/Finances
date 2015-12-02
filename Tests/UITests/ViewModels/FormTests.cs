using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Records;
using UI.ViewModels;
using static NSubstitute.Substitute;
using static Records.Record;
using static Records.Record.Categories;
using static Records.Record.Types;

namespace UITests.ViewModels
{
	public class FormTests : AssertionHelper
	{
		private static Form CreateForm()
		{
			var settings = For<ISettings>();

			settings.CategoriesMapping = new Dictionary<Types, Categories[]>
			{
				[Expense] = new[] {Food, Health, House, General, Women, Other},
				[Debt] =	new[] {Maxim, Andrey},
				[Income] =	new[] {Deposit, ODesk},
				[Shared] =	new[] {Food, House, General, Other},
				[Balance] = new[] {Other}
			};

			return new Form(settings);
		}

		[Test]
		public void Types_Always_ContainsAllMembersOfTypesEnum()
		{
			var form = CreateForm();

			IEnumerable<Types> expected = new []{Balance, Debt, Expense, Income,  Shared, } ;
			Expect(form.Types, EquivalentTo(expected));
		}

		[TestCase(Expense,	new[] { Food, General, Health, House, Other, Women })]
		[TestCase(Debt,		new[] { Maxim, Andrey })]
		[TestCase(Income,	new[] { ODesk, Deposit })]
		[TestCase(Shared,	new[] { Food, House, General, Other })]
		[TestCase(Balance,	new[] { Other })]
		public void SelectedTypeSetter_Always_FiltersAvailableCategories(Types type, Categories[] expected)
		{
			var form = CreateForm();

			form.SelectedType = type;

			Expect(form.Categories, EquivalentTo(expected));
		}
	}
}