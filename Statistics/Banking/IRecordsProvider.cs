using System;
using System.Collections.Generic;
using Common;

namespace Statistics.Banking
{
	public interface IRecordsProvider
	{
		void Get(Action<IEnumerable<Record>> callback);
	}
}