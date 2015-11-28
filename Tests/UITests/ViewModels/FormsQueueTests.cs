using NUnit.Framework;
using UI.ViewModels;

namespace UITests.ViewModels
{
	public class FormsQueueTests : AssertionHelper
	{
		[Test]
		public void CanAdd_LessThanFourForms_ReturnsTrue()
		{
			var forms = new FormsQueue();

			forms.Add();
			var actual = forms.CanAdd();

			Expect(actual, True);
		}

		[Test]
		public void CanAdd_MoreThanFourForms_ReturnsFalse()
		{
			var forms = new FormsQueue();

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
			var forms = new FormsQueue();

			var actual = forms.CanRemove();

			Expect(actual, False);
		}

		[Test]
		public void CanRemove_FewForms_ReturnsTrue()
		{
			var forms = new FormsQueue();

			forms.Add();
			forms.Add();
			forms.Add();
			var actual = forms.CanRemove();

			Expect(actual, True);
		}

		[Test]
		public void CanSubmit_NoForms_ReturnsFalse()
		{
			var forms = new FormsQueue();

			var actual = forms.CanSubmit();

			Expect(actual, False);
		}

	}
}