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

		private FormsQueue Create(IFormFactory factory)
		{
			return new FormsQueue(factory);
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
			var factory = For<IFormFactory>();
			var form = For<IForm>();
			var forms = Create(factory);
			factory.Create().Returns(form);

			forms.Add();

			Expect(forms.Forms, Contains(form));
		}
	}
}