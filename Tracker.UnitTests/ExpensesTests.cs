using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tracker.UnitTests
{
	[TestFixture]
	public class ExpensesTests : AssertionHelper
	{
		//------------------------------------------------------------------
		[Test]
		public void Create_InvalidRecord_ReturnsException ()
		{
			Expect(() => new Record(333, 0, Record.Types.Balance, Record.Categories.Food, "Test", DateTime.Now), Throws.ArgumentException);
			Assert.Catch<ArgumentException>(() => new Record(333, 0, Record.Types.Balance, Record.Categories.Food, "Test", DateTime.Now));
			Assert.Catch<ArgumentException>(() => new Record(333, 10, Record.Types.Balance, Record.Categories.Food, null, DateTime.Now));
			Assert.Catch<ArgumentException>(() => new Record(333, 10, Record.Types.Balance, Record.Categories.Food, "", DateTime.Now));
//			Assert.Catch<ArgumentException>(() => new Record(333, 10, Record.Types.Balance, Record.Categories.Food, "asdd", DateTime.Now.AddDays(-1)));
		}

		//------------------------------------------------------------------
//		[Test]
//        public void AddRecord_Empty_NotFailed ()
//        {
//            Expenses expenses = new Expenses();
//
//	        Assert.Catch<ArgumentNullException>(() => expenses.Add(TODO, TODO, TODO, TODO));
//        }
	}
}