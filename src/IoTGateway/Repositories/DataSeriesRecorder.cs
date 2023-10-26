using IoTGateway.Models;

namespace IoTGateway.Repositories
{
	/// <summary>
	/// This class is used to record data series based on cyclic recording.
	/// </summary>
	/// <typeparam name="T"></typeparam>
    internal sealed class DataSeriesRecorder<T> : IDisposable
	{
		private readonly Timer _timer;
		private readonly IDataSeriesRepository _dataSeriesRepository;
		private readonly TimeSpan _period;
		private readonly Func<T?> _getValueFunc;
		private readonly Reference _reference;
		private DateTime _nextDateTime;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dataSeriesRepository">The target data series repository to record data</param>
		/// <param name="period">Cyclic recording period</param>
		/// <param name="getValueFunc">Function to get the value to record</param>
		/// <param name="reference">Reference of the data series</param>
		public DataSeriesRecorder(IDataSeriesRepository dataSeriesRepository, TimeSpan period, Func<T?> getValueFunc, Reference reference)
		{
			_dataSeriesRepository = dataSeriesRepository;
			_period = period;
			_getValueFunc = getValueFunc;
			_reference = reference;

			_nextDateTime = DateTime.UtcNow;
			
			// We create a timer, that is updated every period.
			_timer = new Timer(Tick, null, 0, Timeout.Infinite);
		}


		public void Dispose()
		{
			_timer.Dispose();
		}

		private void Tick(object? state)
		{
			var now = DateTime.UtcNow;

			while (_nextDateTime < now)
			{
				_dataSeriesRepository.Add(_reference, _nextDateTime, _getValueFunc());
				_nextDateTime += _period;
			}

			_timer.Change(_period, Timeout.InfiniteTimeSpan);
		}
	}
}
