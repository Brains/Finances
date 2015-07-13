using System;

namespace Statistics.Storages
{
	public interface IStorage<T>
	{
		event Action<T> Updated;
		T Value { get; set; }
		string Name { get; set; }
	}
}
