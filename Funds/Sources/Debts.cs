using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Storages;
using static Common.Record;
using static Common.Record.Types;
using Directions = System.Collections.Generic.Dictionary<string, decimal>;

namespace Funds.Sources
{
    public class Debts : IFundsSource
    {
        private readonly IExpenses expenses;

        public Debts(IExpenses expenses)
        {
            this.expenses = expenses;
        }

        public Dictionary<Categories, decimal> Dudes { get; set; }
        public event Action<decimal> Update = delegate { };

        public virtual void PullValue()
        {
            Dudes = Calculate(expenses.Records);
            Update(Dudes.Sum(pair => pair.Value));
        }

        public Dictionary<Categories, decimal> Calculate(IEnumerable<Record> records)
        {
            var types = records.ToLookup(record => record.Type);

            Validate(types[Debt]);

            var directions = CalculateDirections(types[Debt]);
            var shared = types[Shared].Sum(record => record.Amount);

            return directions.ToDictionary(dude => dude.Key,
                dude => shared + dude.Value["Out"] - dude.Value["In"]);
        }

        private Dictionary<Categories, Directions> CalculateDirections(IEnumerable<Record> records)
        {
            return records.ToLookup(record => record.Category)
                .ToDictionary(dude => dude.Key, CalculateTotalForDirections);
        }

        private Directions CalculateTotalForDirections(IGrouping<Categories, Record> dude)
        {
            return dude.ToLookup(record => record.Description)
                .ToDictionary(direction => direction.Key,
                    direction => direction.Sum(record => record.Amount));
        }

        public void Validate(IEnumerable<Record> records)
        {
            var invalid = records.Where(record =>
            {
                var input = record.Description.Equals("In");
                var output = record.Description.Equals("Out");

                return !input && !output;
            });

            if (invalid.Any())
                throw new ArgumentException("Wrong Description for Debt record");
        }
    }
}