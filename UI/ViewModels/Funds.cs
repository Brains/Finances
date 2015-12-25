using System;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using Common;
using Common.Storages;
using MoreLinq;
using UI.Interfaces;
using static Common.Record.Types;

namespace UI.ViewModels
{
	public class Funds : PropertyChangedBase, IViewModel, IHandle<Record>
	{
		private readonly IExpenses expenses;

		public Funds(IFundsSource[] sources, IExpenses expenses, IEventAggregator events)
		{
			if (!sources.Any()) throw new ArgumentException();

			this.expenses = expenses;

			Sources = sources;
			Sources.ForEach(source => source.PropertyChanged += Update);
			Sources.ForEach(source => source.PullValue());

			events.Subscribe(this);
		}

		public IFundsSource[] Sources { get; }
		public decimal Divergence { get; set; }
		public decimal Total { get; set; }
		public int RowIndex { get; } = 0;

		public void Handle(Record message)
		{
			Update(this, null);
		}

		private void Update(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Total = Sources.Sum(source => source.Value);
			Divergence = CalculateDivergence(Total, expenses.Records.ToArray());

			NotifyOfPropertyChange(nameof(Divergence));
			NotifyOfPropertyChange(nameof(Total));
		}

		public decimal CalculateDivergence(decimal real, Record[] records)
		{
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