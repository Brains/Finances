using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using NUnit.Framework;
using Visualization.Banking;

namespace Visualization.Tests
{
	[TestFixture]
	class PrivatBankTests : AssertionHelper
	{
		[Test]
		public void GetBalance()
		{
			PrivatBank bank = new PrivatBank();

			bank.GetBalance(Console.WriteLine);
			Thread.Sleep(2000);
		}

		public void GetHistory()
		{
			PrivatBank bank = new PrivatBank();

			bank.GetHistory(records => records.ForEach(Console.WriteLine));
			Thread.Sleep(3000);
		}
	}
}
