using System;
using System.Collections.Generic;
using Tracker;

namespace Statistics.Banking
{
	public interface IRecordsProvider
	{
		void GetAsync(Action<IEnumerable<Record>> callback);
	}
}