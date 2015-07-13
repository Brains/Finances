using System;
using System.ComponentModel;

namespace Statistics.Storages
{
	public interface IStorage<T> : INotifyPropertyChanged
	{
		string Name { get; set; }
		T Value { get; set; }
	}
}
