using System;
using System.Collections.Generic;
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
	public class Funds : Screen, IViewModel, IHandle<Record>
	{
		private readonly IExpenses expenses;
	    private readonly IEventAggregator events;

	    public Funds(IFundsSource[] sources, IExpenses expenses, IEventAggregator events)
		{
			if (!sources.Any()) throw new ArgumentException();

			this.expenses = expenses;
	        this.events = events;

	        this.Sources = sources;
        }

	    protected override void OnInitialize()
	    {
	        base.OnInitialize();

            events.Subscribe(this);
            Sources.ForEach(source => source.Update += Update);
            Sources.ForEach(source => source.PullValue());
        }

	    public IFundsSource[] Sources { get; }
        public decimal Divergence { get; set; }
		public decimal Total { get; set; }
		public int RowIndex { get; } = 0;

		private void Update()
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

	    public void Handle(Record message)
	    {
            Sources.ForEach(source => source.PullValue());
        }
	}
}