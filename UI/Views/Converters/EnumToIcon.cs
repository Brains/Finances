using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Common;
using Mapping = System.Collections.Generic.Dictionary<string, string>;

namespace UI.Views.Converters
{
	public class EnumToIcon : Base
	{
		readonly Mapping mapping = new Mapping
		{
			["Expense"] = "appbar_arrow_down",
			["Income"] = "appbar_arrow_up",
			["Shared"] = "appbar_share",
			["Debt"] = "appbar_arrow_up_down",
			["Food"] = "appbar_food_silverware",
			["House"] = "appbar_home",
			["Health"] = "appbar_medical_pill",
			["Other"] = "appbar_creditcard",
			["General"] = "appbar_cart",
			["Women"] = "appbar_gender_female",
			["ODesk"] = "appbar_control_guide",
			["Deposit"] = "appbar_money",
			["Maxim"] = "appbar_camera",
			["Andrey"] = "appbar_bike",
		};

		public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			string key = value?.ToString();

			if (string.IsNullOrEmpty(key))
				return null;

			return Application.Current.FindResource(mapping[key]) as Visual;
		}
	}
}