using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Common;
using Common.Storages;
using MoreLinq;
using UI.Interfaces;
using UI.Services;
using static Common.Record.Types;

namespace UI.ViewModels
{
	public class Funds : PropertyChangedBase, IViewModel
	{
		private readonly IExpenses expenses;
		public IFundsSource[] Sources { get; }
		public decimal Divergence { get; set; }
		public decimal Total { get; set; }
		public int RowIndex { get; } = 0;

		public Funds(IFundsSource[] sources, IExpenses expenses)
		{
			if (!sources.Any()) throw new ArgumentException();

			this.expenses = expenses;

			Sources = sources;
			Sources.ForEach(source => source.PropertyChanged += Update);
			Sources.ForEach(source => source.PullValue());
		}

		private void Update(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Divergence = CalculateDivergence(Sources, expenses.Records.ToArray());
			Total = Sources.Sum(source => source.Value);

			NotifyOfPropertyChange(nameof(Divergence));
			NotifyOfPropertyChange(nameof(Total));
		}

		public decimal CalculateDivergence(IFundsSource[] sources, Record[] records)
		{
			var real = sources.Sum(source => source.Value);
			var estimated = CalculateEstimatedBalance(records);

			return real - estimated;
		}

		public decimal CalculateEstimatedBalance(Record[] records)
		{
			var totals = records.ToLookup(record => record.Type)
			                   .ToDictionary(type => type.Key,
			                                 type => type.Sum(record => record.Amount));

			var spending = totals[Expense] + totals[Shared];
			var income = totals[Income];

			return income - spending;
		}
	}
}