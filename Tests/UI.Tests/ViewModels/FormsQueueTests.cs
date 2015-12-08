using System.Linq;
using Caliburn.Micro;
using NSubstitute;
using NUnit.Framework;
using UI.Interfaces;
using UI.ViewModels;
using static NSubstitute.Substitute;
using Handler = System.ComponentModel.PropertyChangedEventHandler;
using Args = System.ComponentModel.PropertyChangedEventArgs;

namespace UI.Tests.ViewModels
{
	public class FormsQueueTests : AssertionHelper
	{
		private FormsQueue Create()
		{
			return new FormsQueue(For<IFormFactory>());
		}

		private BindableCollection<IForm> GetForms(int formsCount, int defaultAmount = 10)
		{
			var forms = Enumerable.Range(1, formsCount)
			                           .Select(index => CreateForm(defaultAmount))
			                           .ToList();

			return new BindableCollection<IForm>(forms);
		}

		private static IForm CreateForm(int defaultAmount)
		{
			var form = For<IForm>();
			form.Amount = defaultAmount;

			return form;
		}

		[Test]
		public void CanAdd_LessThanFourForms_ReturnsTrue()
		{
			var forms = Create();

			forms.Add();

			Expect(forms.CanAdd, True);
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

			Expect(forms.CanAdd, False);
		}

		[Test]
		public void CanRemove_NoForms_ReturnsFalse()
		{
			var forms = Create();

			Expect(forms.CanRemove, False);
		}

		[Test]
		public void CanRemove_FewForms_ReturnsTrue()
		{
			var forms = Create();

			forms.Add();
			forms.Add();
			forms.Add();

			Expect(forms.CanRemove, True);
		}

		[Test]
		public void CanSubmit_NoForms_ReturnsFalse()
		{
			var forms = Create();

			Expect(forms.CanSubmit, False);
		}

		[Test]
		public void CanSubmit_OneForm_ReturnsTrue()
		{
			var forms = Create();
			forms.Add();

			Expect(forms.CanSubmit, True);
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
		public void Add_Always_RaisesAllPropertiesChanged()
		{
			var forms = Create();
			var handler = For<Handler>();
			forms.PropertyChanged += handler;

			forms.Add();

			handler.Received(1).Invoke(forms, Arg.Is<Args>(args => args.PropertyName == "CanAdd"));
			handler.Received(1).Invoke(forms, Arg.Is<Args>(args => args.PropertyName == "CanRemove"));
			handler.Received(1).Invoke(forms, Arg.Is<Args>(args => args.PropertyName == "CanSubmit"));
		}

		[Test]
		public void Remove_Always_RemovesLastForm()
		{
			var forms = Create();
			forms.Forms = GetForms(3);
			var last = forms.Forms.Last();

			forms.Remove();

			Expect(forms.Forms, Not.Contains(last));
			Expect(forms.Forms, Count.EqualTo(2));
		}

		[Test]
		public void Remove_Always_RaisesAllPropertiesChanged()
		{
			var forms = Create();
			forms.Forms = GetForms(1);
			var handler = For<Handler>();
			forms.PropertyChanged += handler;

			forms.Remove();

			handler.Received(1).Invoke(forms, Arg.Is<Args>(args => args.PropertyName == "CanAdd"));
			handler.Received(1).Invoke(forms, Arg.Is<Args>(args => args.PropertyName == "CanRemove"));
			handler.Received(1).Invoke(forms, Arg.Is<Args>(args => args.PropertyName == "CanSubmit"));
		}

		[Test]
		public void Submit_Always_CallsSubmitForEachForm()
		{
			var forms = Create();
			forms.Add();
			forms.Add();
			forms.Add();
			var saved = forms.Forms.ToList();

			forms.Submit();

			saved[0].Received().Submit();
			saved[1].Received().Submit();
			saved[2].Received().Submit();
		}

		[Test]
		public void Submit_SingleForm_PreservesItsAmount()
		{
			var forms = Create();
			forms.Forms = GetForms(1, 100);
			var saved = forms.Forms.ToList();

			forms.Submit();

			Expect(saved[0].Amount, EqualTo(100));
		}

		[Test]
		public void Submit_FewForms_RemovesThemAll()
		{
			var forms = Create();
			forms.Forms = GetForms(3);

			forms.Submit();

			Expect(forms.Forms, Empty);
		}

		[Test]
		public void Submit_Always_RaisesAllPropertiesChanged()
		{
			var forms = Create();
			forms.Forms = GetForms(1);
			var handler = For<Handler>();
			forms.PropertyChanged += handler;

			forms.Submit();

			handler.Received(1).Invoke(forms, Arg.Is<Args>(args => args.PropertyName == "CanAdd"));
			handler.Received(1).Invoke(forms, Arg.Is<Args>(args => args.PropertyName == "CanRemove"));
			handler.Received(1).Invoke(forms, Arg.Is<Args>(args => args.PropertyName == "CanSubmit"));
		}

		[Test]
		public void Submit_FewForms_SubstractsSubsequentFormsFromFirstOne()
		{
			var forms = Create();
			forms.Forms = GetForms(3);
			forms.Forms[0].Amount = 100;
			var saved = forms.Forms.ToList();

			forms.Submit();

			Expect(saved[0].Amount, EqualTo(80));
			Expect(saved[1].Amount, EqualTo(10));
			Expect(saved[2].Amount, EqualTo(10));
		}
	}
}