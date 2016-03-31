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
		private readonly DateTime date = new DateTime(1, 1, 1);

        private Trend Create() => new Trend(For<IExpenses>());
	    private DateTime Month(int month) => new DateTime(1, month, 1);


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

        [TestCase(Record.Types.Expense, -100)]
        [TestCase(Record.Types.Shared, -100)]
        [TestCase(Record.Types.Income, 100)]
	    public void GetAmount_ForRecordType_ReturnRightAmountWithSign(Record.Types type, int expected)
	    {
            var trend = Create();
            Record record = new Record(100, type, 0, "", date);

            var actual = trend.GetAmount(record);

            Assert.That(actual, Is.EqualTo(expected));
	    }

        [TestCase(Record.Types.Debt, "Out", -100)]
        [TestCase(Record.Types.Debt, "In", 100)]
	    public void GetAmount_ForDebtType_ReturnRightAmountWithSign(Record.Types type, string description, int expected)
	    {
            var trend = Create();
            Record record = new Record(100, type, 0, description, date);

            var actual = trend.GetAmount(record);

            Assert.That(actual, Is.EqualTo(expected));
	    }
	}
}