using System;

namespace Trends
{
	public struct DatePeriod
	{
		enum Types
		{
			Days,
			Months,
		}

		private int units;
		private Types type;

		private DatePeriod(Types type, int units)
		{
			CodeContracts.Requires.True(units > 0, "units > 0");

			this.units = units;
			this.type = type;
		}

		public static DatePeriod FromDays(int units)
		{
			return new DatePeriod(Types.Days, units);
		}

		public static DatePeriod FromMonths(int units)
		{
			return new DatePeriod(Types.Months, units);
		}

		public DateTime Next(DateTime from)
		{
			CodeContracts.Requires.True(units > 0, "units > 0");

			if (type == Types.Days)
				return from.AddDays(units);
			if (type == Types.Months)
				return @from.AddMonths(units);

			return DateTime.MinValue;
		}
	}
}