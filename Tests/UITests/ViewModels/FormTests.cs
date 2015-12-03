using System;
using System.Collections;
using System.Collections.Generic;
using NSubstitute;
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
		private static ISettings settings;
		private static IRecordsStorage storage;

		private static Form CreateForm()
		{
			settings = For<ISettings>();
			storage = For<IRecordsStorage>();

			settings.CategoriesMapping = new Dictionary<Types, Categories[]>
			{
				[Expense]	= new[] {Food, Health, House, General, Women, Other},
				[Debt]		= new[] {Maxim, Andrey},
				[Income]	= new[] {Deposit, ODesk},
				[Shared]	= new[] {Food, House, General, Other},
			};

			return new Form(settings, storage);
		}

		[Test]
		public void Types_Always_ContainsAllMembersOfTypesEnum()
		{
			var form = CreateForm();

			IEnumerable<Types> expected = new []{Debt, Expense, Income,  Shared} ;
			Expect(form.Types, EquivalentTo(expected));
		}

		[TestCase(Expense,	new[] { Food, General, Health, House, Other, Women })]
		[TestCase(Debt,		new[] { Maxim, Andrey })]
		[TestCase(Income,	new[] { ODesk, Deposit })]
		[TestCase(Shared,	new[] { Food, House, General, Other })]
		public void SelectedTypeSetter_Always_FiltersAvailableCategories(Types type, Categories[] expected)
		{
			var form = CreateForm();

			form.SelectedType = type;

			Expect(form.Categories, EquivalentTo(expected));
		}

		[Test]
		public void Submit_Always_CallsAggregatorAdd()
		{
			var form = CreateForm();

			form.Submit();

			var record = new Record(form.Amount, form.SelectedType, form.SelectedCategory, form.Description, form.DateTime);
			storage.Received().Add(record);
		}

		[Test]
		public void Submit_Shared_DividesAmountIntoThree()
		{
			var form = CreateForm();
			form.SelectedType = Shared;
			form.Amount = 30;

			form.Submit();

			storage.Received().Add(Arg.Is<Record>(record => record.Amount == 10));
		}

		[TestCase(Expense)]
		[TestCase(Debt)]
        [TestCase(Income)]
		public void Submit_AllTypesExceptShared_DontChangeAmount(Types selectedType)
		{
			var form = CreateForm();
			form.SelectedType = selectedType;
			form.Amount = 10;

			form.Submit();

			storage.Received().Add(Arg.Is<Record>(record => record.Amount == 10));
		}
	}
}