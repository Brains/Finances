using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Common;
using MoreLinq;
using UI.Interfaces;
using UI.Services;
using static Common.Record.Types;

namespace UI.ViewModels
{
	public class Funds : PropertyChangedBase, IViewModel
	{
		public IFundsSource[] Sources { get; }

		public Funds(IFundsSource[] sources)
		{
			if (!sources.Any()) throw new ArgumentException();

			Sources = sources;
			Sources.ForEach(source => source.PullValue());
		}

		public decimal CalculateDivergence(IFundsSource[] sources, Record[] records)
		{
			var real = sources.Sum(source => source.Value);
			var estimated = CalculateEstimatedBalance(records);

			return real - estimated;
		}

		public decimal CalculateEstimatedBalance(Record[] records)
		{
			var types = records.ToLookup(record => record.Type)
			                   .ToDictionary(type => type.Key,
			                                 type => type.Sum(record => record.Amount));

			var spending = types[Expense] + types[Shared];
			var income = types[Income];

			return income - spending;
		}
	}
}