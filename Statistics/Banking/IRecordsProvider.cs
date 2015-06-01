using System;
using System.Collections.Generic;
using Tracker;

namespace Visualization.Banking
{
	public interface IRecordsProvider
	{
		void GetHistoryAsync(Action<IEnumerable<Record>> callback);
	}
}