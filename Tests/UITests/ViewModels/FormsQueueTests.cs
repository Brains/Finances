using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UI.Interfaces;
using UI.ViewModels;
using static NSubstitute.Substitute;

namespace UITests.ViewModels
{
	public class FormsQueueTests : AssertionHelper
	{
		private FormsQueue Create()
		{
			return new FormsQueue(For<IFormFactory>());
		}

		private void ConfigureFormSeries(IFormFactory factory)
		{
			var primary = For<IForm>();
			var secondary = For<IForm>();
			primary.Amount = 100;
			secondary.Amount = 10;

			factory.Create().Returns(primary, secondary, secondary);
		}

		[Test]
		public void CanAdd_LessThanFourForms_ReturnsTrue()
		{
			var forms = Create();

			forms.Add();
			var actual = forms.CanAdd();

			Expect(actual, True);
		}

		[Test]
		public void CanAdd_MoreThanFourForms_ReturnsFalse()
		{
			var forms = Create();

			forms.Add();
			forms.Add();
			forms.Add();
			forms.Add();
			forms.Add();
			var actual = forms.CanAdd();

			Expect(actual, False);
		}

		[Test]
		public void CanRemove_NoForms_ReturnsFalse()
		{
			var forms = Create();

			var actual = forms.CanRemove();

			Expect(actual, False);
		}

		[Test]
		public void CanRemove_FewForms_ReturnsTrue()
		{
			var forms = Create();

			forms.Add();
			forms.Add();
			forms.Add();
			var actual = forms.CanRemove();

			Expect(actual, True);
		}

		[Test]
		public void CanSubmit_NoForms_ReturnsFalse()
		{
			var forms = Create();

			var actual = forms.CanSubmit();

			Expect(actual, False);
		}

		[Test]
		public void Add_Always_AddsFormFromFactory()
		{
			var form = For<IForm>();
			var forms = Create();
			forms.Factory.Create().Returns(form);

			forms.Add();

			Expect(forms.Forms, Contains(form));
		}

		[Test]
		public void Remove_Always_RemovesLastForm()
		{
			var forms = Create();
			forms.Forms = new List<IForm>
			{
				For<IForm>(),
				For<IForm>(),
				For<IForm>(),
			};
			var last = forms.Forms.Last();

			forms.Remove();

			Expect(forms.Forms, Not.Contains(last));
			Expect(forms.Forms, Count.EqualTo(2));
		}

		[Test]
		public void Submit_Always_CallsSubmitForEachForm()
		{
			var forms = Create();
			forms.Add();
			forms.Add();
			forms.Add();

			forms.Submit();

			forms.Forms[0].Received().Submit();
			forms.Forms[1].Received().Submit();
			forms.Forms[2].Received().Submit();
        }

		[Test]
		public void Submit_Always_SubstractsSubsequentFormsFromFirstOne()
		{
			var forms = Create();
			ConfigureFormSeries(forms.Factory);

			forms.Add();
			forms.Add();
			forms.Add();

			forms.Submit();

			Expect(forms.Forms[0].Amount, EqualTo(80));
			Expect(forms.Forms[1].Amount, EqualTo(10));
			Expect(forms.Forms[2].Amount, EqualTo(10));
        }
	}
}