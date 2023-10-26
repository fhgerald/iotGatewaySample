using System.Collections.Concurrent;
using IoTGateway.Models;

namespace IoTGateway.Repositories
{
	/// <summary>
	/// Implements a data series repository in memory.
	/// </summary>
	/// <typeparam name="T">Type of value we use</typeparam>
	internal class InMemoryDataSeriesRepository : IDataSeriesRepository
	{
		private readonly ConcurrentDictionary<Reference, ConcurrentDictionary<DateTime, object?>> _dataSeries = new();

		public void Add(Reference reference, DateTime date, object? value)
		{
			var dataPoints = _dataSeries.GetOrAdd(reference, new ConcurrentDictionary<DateTime, object?>());
			dataPoints.AddOrUpdate(date, value, (_, _) => value);

		}

		public Task<IEnumerable<Reference>> GetReferencesAsync(int? skip = null, int? take = null)
		{
			return Task.FromResult((IEnumerable<Reference>)_dataSeries.Keys);
		}

		public Task<IEnumerable<DataPoint>> GetDataPointsAsync(Reference reference, DateTime startDateTime, DateTime endDateTime, int? skip = null,
			int? take = null)
		{
			var dataPoints = _dataSeries.GetOrAdd(reference, new ConcurrentDictionary<DateTime, object?>());
			var pairs = Volatile.Read(ref dataPoints)
				.SkipWhile(p => p.Key < startDateTime)
				.TakeWhile(p => p.Key <= endDateTime);

			if (skip != null)
			{
				pairs = pairs.Skip(skip.Value);
			}
			if (take != null)
			{
				pairs = pairs.Take(take.Value);
			}
			
			return Task.FromResult(pairs.Select(kvp => new DataPoint(reference, kvp.Key, kvp.Value)));

		}

		public virtual void Dispose()
		{
			// Intentionally left empty. In memory repository does not need to dispose anything.
		}
		
	}
}
