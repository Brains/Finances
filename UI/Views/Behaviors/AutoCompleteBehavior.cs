// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
// Author: https://github.com/Nimgoble/WPFTextBoxAutoComplete/. Thank you
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.StringComparison;

namespace UI.Views.Behaviors
{
	public static class AutoCompleteBehavior
	{
		private static readonly TextChangedEventHandler onTextChanged = OnTextChanged;
		private static readonly KeyEventHandler onKeyDown = OnPreviewKeyDown;

		public static readonly DependencyProperty AutoCompleteItemsSource =
			DependencyProperty.RegisterAttached
				(
					"AutoCompleteItemsSource",
					typeof (IEnumerable<String>),
					typeof (AutoCompleteBehavior),
					new UIPropertyMetadata(null, OnAutoCompleteItemsSource)
				);

		public static IEnumerable<String> GetAutoCompleteItemsSource(DependencyObject obj)
		{
			object value = obj.GetValue(AutoCompleteItemsSource);
			return value as IEnumerable<string>;
		}

		public static void SetAutoCompleteItemsSource(DependencyObject obj, IEnumerable<String> value)
		{
			obj.SetValue(AutoCompleteItemsSource, value);
		}

		private static void OnAutoCompleteItemsSource(object sender, DependencyPropertyChangedEventArgs args)
		{
			TextBox textBox = sender as TextBox;
			if (sender == null)
				return;

			if (args.NewValue == null)
			{
				textBox.TextChanged -= onTextChanged;
				textBox.PreviewKeyDown -= onKeyDown;
			}
			else
			{
				textBox.TextChanged += onTextChanged;
				textBox.PreviewKeyDown += onKeyDown;
			}
		}

		private static void OnPreviewKeyDown(object sender, KeyEventArgs args)
		{
			if (args.Key != Key.Enter)
				return;

			TextBox textBox = args.OriginalSource as TextBox;
			if (textBox == null)
				return;

			//If we pressed enter and if the selected text goes all the way to the end, move our caret position to the end
			if (textBox.SelectionLength > 0 && (textBox.SelectionStart + textBox.SelectionLength == textBox.Text.Length))
			{
				textBox.SelectionStart = textBox.CaretIndex = textBox.Text.Length;
				textBox.SelectionLength = 0;
			}
		}

		private static void OnTextChanged(object sender, TextChangedEventArgs args)
		{
			var removed = from change in args.Changes
						  where change.RemovedLength > 0
						  select change;

			var added = from change in args.Changes
						where change.AddedLength > 0
						select change;

			if (removed.Any() && !added.Any())
				return;

			TextBox textBox = args.OriginalSource as TextBox;
			if (sender == null)
				return;

			var values = GetAutoCompleteItemsSource(textBox);
			//No reason to search if we don't have any values.
			if (values == null)
				return;

			//No reason to search if there's nothing there.
			if (String.IsNullOrEmpty(textBox.Text))
				return;

			Int32 textLength = textBox.Text.Length;

			//Do search and changes here.
			var matches = from value in (from subvalue in values
			                             where subvalue.Length >= textLength
			                             select subvalue)
			              where string.Equals(value.Substring(0, textLength), textBox.Text, CurrentCultureIgnoreCase)
			              select value;

			//Nothing.  Leave 'em alone
			if (!matches.Any()) return;

			String match = matches.ElementAt(0);
			//String remainder = match.Substring(textLength, (match.Length - textLength));
			textBox.TextChanged -= onTextChanged;
			textBox.Text = match;
			textBox.CaretIndex = textLength;
			textBox.SelectionStart = textLength;
			textBox.SelectionLength = (match.Length - textLength);
			textBox.TextChanged += onTextChanged;
		}
	}
}


// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
// Author: https://github.com/Nimgoble/WPFTextBoxAutoComplete/. Thank you
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------