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
	    private ISettings settings;

	    private Trend Create()
	    {
	        settings = For<ISettings>();
	        return new Trend(For<IExpenses>(), settings);
	    }

	    private DateTime Month(int month) => new DateTime(1, month, 1);
	    private DateTime Day(int day) => new DateTime(1, 1, day);


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
                var transaction = new Trend.Transaction()
                {
                    Date = record.Date
                };
                return trend.IsShown(transaction);
            }).ToArray();

            Assert.That(actual, Is.EqualTo(new [] {false, false , true}));
	    }

	    [TestCase(Record.Types.Expense, -100)]
        [TestCase(Record.Types.Income, 100)]
	    public void GetAmount_ForRecordType_ReturnRightAmountWithSign(Record.Types type, int expected)
	    {
            var trend = Create();
	        settings.Customers = 3;
            Record record = new Record(100, type, 0, "", date);

            var actual = trend.GetAmount(record);

            Assert.That(actual, Is.EqualTo(expected));
	    }

	    public void GetAmount_ForSharedType_ReturnRightAmountWithSign()
	    {
            var trend = Create();
	        settings.Customers = 3;
            Record record = new Record(100, Record.Types.Shared, 0, "", date);

            var actual = trend.GetAmount(record);

            Assert.That(actual, Is.EqualTo(-300));
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

	    [Test]
	    public void CombineByDay_FewRecordsWithSameDate_GroupsThemTogether()
	    {
	        var trend = Create();
	        Record[] records =
	        {
	            new Record(0, 0, 0, "", Day(1)),
	            new Record(0, 0, 0, "", Day(1)),
	            new Record(0, 0, 0, "", Day(2)),
	            new Record(0, 0, 0, "", Day(2)),
	            new Record(0, 0, 0, "", Day(3)),
	            new Record(0, 0, 0, "", Day(3)),
	        };

	        var actual = trend.CombineByDay(records)
	                          .Select(record => record.Date)
	                          .ToList();

	        Assert.That(actual, Has.Count.EqualTo(3));
	        Assert.That(actual, Is.Unique);
	    }

	    [Test]
	    public void CombineByDay_FewRecordsWithSameDate_CalculatesSumOfTheirAmounts()
	    {
	        var trend = Create();
	        Record[] records =
	        {
	            new Record(100, 0, 0, "", Day(1)),
	            new Record(100, 0, 0, "", Day(1)),
	            new Record(100, 0, 0, "", Day(1)),
	        };

	        var actual = trend.CombineByDay(records)
	                          .Select(record => record.Amount)
	                          .Single();

	        Assert.That(actual, Is.EqualTo(-300));
	    }

	    [Test]
	    public void CombineByDay_FewRecordsWithSameDate_AggregatesTheirDescriptions()
	    {
	        var trend = Create();
	        Record[] records =
	        {
	            new Record(0, 0, 0, "Test", Day(1)),
	            new Record(0, 0, 0, "Test", Day(1)),
	            new Record(0, 0, 0, "Test", Day(1)),
	        };

	        var actual = trend.CombineByDay(records)
	                          .Select(record => record.Description)
	                          .Single();

	        Assert.That(actual, Is.EqualTo("Test\nTest\nTest"));
	    }

	    [Test]
	    public void Calculate_Always_ReturnsCollectionOfTotals()
	    {
	        var trend = Create();
	        settings.Customers = 3;
	        trend.Interval = 20;
            trend.Now = date.AddDays(10);
            Record[] records =
	        {
	            new Record(1000, Record.Types.Income, 0, "", Day(1)),
	            new Record(100, Record.Types.Expense, 0, "", Day(2)),
	            new Record(100, Record.Types.Shared, 0, "", Day(3)),
	            new Record(100, Record.Types.Debt, 0, "Out", Day(4)),
	            new Record(100, Record.Types.Debt, 0, "In", Day(5)),
	        };

	        var actual = trend.Calculate(records)
	                          .Select(transaction => transaction.Total)
                              .ToArray();

	        Assert.That(actual, Is.EqualTo(new [] {1000, 900, 600, 500, 600}));
	    }
	}
}