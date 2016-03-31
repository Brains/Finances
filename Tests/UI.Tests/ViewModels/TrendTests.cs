using System;
using System.Linq;
using Common;
using Common.Storages;
using MoreLinq;
using NUnit.Framework;
using UI.ViewModels;
using static NSubstitute.Substitute;

namespace UI.Tests.ViewModels
{
	public class TrendTests
	{
		private static Trend Create()
		{
			var settings = For<ISettings>();

			return new Trend(For<IExpenses>(), settings);
		}
	}
}