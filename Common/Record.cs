using System;
using System.Xml.Serialization;

namespace Common
{
	[Serializable]
	public class Record
	{
		public enum Types
		{
			Expense,
			Income,
			Shared,
			Debt
		}

		public enum Categories
		{
			Food,
			House,
			Health,
			Other,
			General,
			Women,

			ODesk,
			Deposit,

			Maxim,
			Andrey
		}

		public Record() {}

		public Record(decimal amount, Types type, Categories category, string description, DateTime date)
		{
			Type = type;
			Amount = amount;
			Category = category;
			Description = description;
			Date = date;
		}

		[XmlAttribute]
		public decimal Amount { get; set; }

		[XmlAttribute]
		public Types Type { get; set; }

		[XmlAttribute]
		public Categories Category { get; set; }

		[XmlAttribute]
		public string Description { get; set; }

		[XmlIgnore]
		public DateTime Date { get; set; }

		[XmlAttribute("Date")]
		public string DateFormatted
		{
			get { return Date.ToString("dd.MM.yy HH:mm:ss"); }
			set { Date = DateTime.Parse(value); }
		}

		public override string ToString()
		{
			return $"{Amount}; {Type}; {Category}; {Description}; {Date.ToString("g")}";
		}

		public override bool Equals(object other)
		{
			var compared = (Record) other;

			return compared.Amount == Amount
			       && compared.Type == Type
			       && compared.Category == Category
			       && compared.Description == Description
			       && compared.Date == Date;
		}
	}
}