using System;
using System.ComponentModel;

namespace Statistics.Storages
{
	public interface IStorage<T> : INotifyPropertyChanged
	{
		T Value { get; set; }
	}
}
