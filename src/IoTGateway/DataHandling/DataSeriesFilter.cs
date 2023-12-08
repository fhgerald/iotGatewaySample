
using IoTGateway.Models;

namespace IoTGateway.DataHandling
{
	/// <summary>
	/// Filters for raw time series data.
	/// </summary>
	internal static class DataSeriesFilter
	{
		/// <summary>
		/// Filters data points based on the given time range and sampling interval
		/// </summary>
		/// <param name="dataPoints">Time-series raw data points</param>
		/// <param name="reference">Reference</param>
		/// <param name="startDateTime">Start time of filter</param>
		/// <param name="endDateTime">End time of filter</param>
		/// <param name="samplingInterval">Sampling interval</param>
		/// <param name="smoothingWindow">Smoothing window</param>
		/// <typeparam name="T"></typeparam>
		/// <returns>Filtered and aggregated data points (based on sampling interval) for the given time range.</returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static IEnumerable<DataPoint> FilterDataPoints(this IEnumerable<DataPoint> dataPoints, string reference, DateTime startDateTime, 
			DateTime endDateTime, TimeSpan samplingInterval, TimeSpan smoothingWindow)
		{
			if (smoothingWindow.Ticks <= 0) throw new ArgumentOutOfRangeException(nameof(smoothingWindow));
			if (samplingInterval.Ticks <= 0) throw new ArgumentOutOfRangeException(nameof(samplingInterval));

			var duration = (endDateTime - startDateTime);

			if (duration.Ticks < 0) throw new ArgumentException();

			var points = new(double Value, double Count)[checked((int)Math.Max(1, unchecked(duration.Ticks / samplingInterval.Ticks)))];

			long halfWindowTicks = Math.Max(1, smoothingWindow.Ticks >> 1);

			int firstPointIndex = 0;
			DateTime firstPointDateTime = startDateTime;

			foreach (var dataPoint in dataPoints)
			{
				int pointIndex;
				DateTime pointDateTime;

				for (pointIndex = firstPointIndex, pointDateTime = firstPointDateTime; pointIndex < points.Length && pointDateTime < endDateTime;
				     pointIndex++, pointDateTime += samplingInterval)
				{
					long distance = (dataPoint.DateTime - pointDateTime).Ticks;

					if (distance < -halfWindowTicks)
					{
						break;
					}

					if (distance > halfWindowTicks)
					{
						firstPointIndex = pointIndex + 1;
						firstPointDateTime = pointDateTime + samplingInterval;
						continue;
					}

					double d = (double)distance / halfWindowTicks;

					if (dataPoint.Value != null)
					{
						var value = Convert.ToDouble(dataPoint.Value);
						Aggregate(ref points[pointIndex], LinearInterpolate(value, d), d);
					}
				}

				if (firstPointIndex >= points.Length) break;
			}

			return GetDataPointEnumerable(reference, points, startDateTime, samplingInterval);
		}

		private static IEnumerable<DataPoint> GetDataPointEnumerable(Reference reference, (double Value, double Count)[] points, 
			DateTime startDateTime, TimeSpan samplingInterval)
		{
			int pointIndex;
			DateTime pointDateTime;

			for (pointIndex = 0, pointDateTime = startDateTime; pointIndex < points.Length; pointIndex++, pointDateTime += samplingInterval)
			{
				yield return CreateDataPoint(reference, ref points[pointIndex], pointDateTime);
			}
		}

		private static DataPoint CreateDataPoint(Reference reference, ref (double Value, double Count) point, DateTime dateTime) =>
			new(reference, dateTime, point.Count > 0 ? point.Value / point.Count : 0);

		private static double LinearInterpolate(double value, double distance) => value * (1d - Math.Abs(distance));
		
		private static void Aggregate(ref (double Value, double Count) point, double value, double distance)
		{
			point.Value += value;
			point.Count += 1d - Math.Abs(distance);
		}
	}
}
