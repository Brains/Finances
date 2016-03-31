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
			return new Trend(For<IExpenses>());
		}
        private static DateTime Month(int month)
        {
            return new DateTime(1, month, 1);
        }


        [Test]
	    public void IsShown_Within10Days_ReturnsTrueForTranscation()
	    {
            var trend = Create();
            trend.Now = Month(3);
            trend.Interval = 10;
            Record[] records =
            {
                new Record(0, 0, 0, "", Month(1)),
                new Record(0, 0, 0, "", Month(2)),
                new Record(0, 0, 0, "", Month(3)),
            };

            var actual = records.Select(record =>
            {
                var transaction = new Trend.Transaction(0, record);
                return trend.IsShown(transaction);
            }).ToArray();

            Assert.That(actual, Is.EqualTo(new [] {false, false , true}));
	    }
	}
}