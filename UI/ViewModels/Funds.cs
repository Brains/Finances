using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Common;
using Common.Storages;
using MoreLinq;
using UI.Interfaces;
using UI.Services;
using static Common.Record.Types;

namespace UI.ViewModels
{
	public class Funds : Screen, IViewModel, IHandle<Record>
	{
		private readonly IExpenses expenses;
	    private readonly IEventAggregator events;

	    public Funds(IFund[] sources, IExpenses expenses, IEventAggregator events)
		{
			if (!sources.Any()) throw new ArgumentException();
            Sources = sources;

            this.expenses = expenses;
	        this.events = events;
        }

	    protected override void OnInitialize()
	    {
	        base.OnInitialize();

            events.Subscribe(this);

            Sources.ForEach(source => source.Updated += value => Update());
            Sources.ForEach(source => source.PullValue());
        }

	    public IFund[] Sources { get; }
	    public int RowIndex { get; } = 0;
	    [Notify] public decimal Divergence { get; set; }
	    [Notify] public decimal Total { get; set; }

	    private void Update()
		{
		    Total = Sources.Sum(source => source.Value);
			Divergence = CalculateDivergence(Total, expenses.Records.ToArray());
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
            Update();
        }
	}
}