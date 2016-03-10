using System;
using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Common;
using Common.Storages;
using UI.Services;
using UI.Services.Amount;
using UI.ViewModels;
using UI.Views.Converters;
using static NSubstitute.Substitute;
using static Common.Record;
using static Common.Record.Categories;
using static Common.Record.Types;

namespace UI.Tests.ViewModels
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

			return new Form(settings, storage, For<IAdder>(), For<IAmountFactory>());
		}

		private static Form CreateValidForm()
		{
			var form = CreateForm();

			form.Amount = "1";
			form.Description = "Test";

			return form;
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

			var record = new Record(Convert.ToDecimal(form.Amount), form.SelectedType, form.SelectedCategory, form.Description, form.DateTime);
			storage.Received().Add(record);
		}

		[TestCase(1.468)]
		[TestCase(10)]
		[TestCase(200)]
		[TestCase(8000)]
		public void CanSubmit_CorrectAmount_ReturnsTrue(decimal amount)
		{
			var form = CreateValidForm();
			form.Amount = amount.ToString();

			var actual = form.CanSubmit();

			Assert.That(actual, True);
		}

		[TestCase(0)]
		[TestCase(0.4648)]
		[TestCase(-1)]
		[TestCase(-10)]
		public void CanSubmit_WrongAmount_ReturnsFalse(decimal amount)
		{
			var form = CreateValidForm();
			form.Amount = amount.ToString();

			var actual = form.CanSubmit();

			Assert.That(actual, False);
		}

		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void CanSubmit_WrongDescription_ReturnsFalse(string description)
		{
			var form = CreateValidForm();
			form.Description = description;

			var actual = form.CanSubmit();

			Assert.That(actual, False);
		}

		[Test]
		public void CanSubmit_CorrectDescription_ReturnsTrue()
		{
			var form = CreateValidForm();
			form.Description = "Correct";

			var actual = form.CanSubmit();

			Assert.That(actual, True);
		}

		[Ignore("ToDo")]
		[Test]
		public void CanSubmit_WrongDescription_Throws()
		{
			var form = CreateValidForm();
			form.Description = null;

			Assert.That(()=> form.CanSubmit(), Throws.Exception);
		}

		[Test]
		public void CanSubmit_DebtInvalidDescription_ReturnsFalse()
		{
			var form = CreateValidForm();
			form.SelectedType = Debt;
			form.Description = "Invalid";

			var actual = form.CanSubmit();

			Assert.That(actual, False);
		}

		[TestCase("In")]
		[TestCase("Out")]
		public void CanSubmit_DebtValidDescription_ReturnsTrue(string description)
		{
			var form = CreateValidForm();
			form.SelectedType = Debt;
			form.Description = description;

			var actual = form.CanSubmit();

			Assert.That(actual, True);
		}
	}
}