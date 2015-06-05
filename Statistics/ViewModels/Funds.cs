using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Practices.Prism.Commands;
using Statistics.Banking;
using Tracker;
using Unity = Microsoft.Practices.Unity;

namespace Statistics.ViewModels
{
	public class Funds : INotifyPropertyChanged
	{
		public const int ExchangeRate = 21;

		public event PropertyChangedEventHandler PropertyChanged;

		public ICommand WindowLoaded { get; private set; }

		private IExpenses expenses;

		private int cards;
		private int debts;
		private int cash;
		private int upwork;
		private int total;
		private int available;

		public int Upwork
		{
			get { return upwork; }
			set { upwork = value; OnPropertyChanged(); }
		}

		public int Cards
		{
			get { return cards; }
			set { cards = value; OnPropertyChanged(); }
		}

		public int Cash
		{
			get { return cash; }
			set { cash = value; OnPropertyChanged(); }
		}

		public int Debts
		{
			get { return debts; }
			set { debts = value; OnPropertyChanged(); }
		}

		public int Total
		{
			get { return total; }
			set { total = value; OnPropertyChanged(); }
		}

		public int Available
		{
			get { return available; }
			set { available = value; OnPropertyChanged(); }
		}

		public Funds(IExpenses expenses, [Unity.Dependency("bank")] IFundsStorage bank, [Unity.Dependency("debt")]IFundsStorage debt)
		{
			PropertyChanged += Update;

			this.expenses = expenses;

			bank.Get(amount => Cards = (int) amount);
			debt.Get(amount => Debts = (int) amount);

			WindowLoaded = new DelegateCommand<object>(o => Load());
		}

		private void UpdateTotal(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == nameof(Total)) return;

			var upworkUAH = Upwork * ExchangeRate;
			Total = upworkUAH + Cards + Cash + Debts;
		}

		public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void Load()
		{
			var path = Path.Combine("Data", "Funds.xml");

			XElement file = XElement.Load(path);

			Upwork = int.Parse(file.Element("Upwork").Value);
			Cash = int.Parse(file.Element("Cash").Value);
		}
	}
}