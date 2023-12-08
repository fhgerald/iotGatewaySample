using System.Text.Json.Serialization;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace IoTGateway.ViewModels;

internal struct TimeSeriesViewModel
{
	public TimeSeriesViewModel(string target, object?[][] dataPoints)
	{
		Target = target;
		DataPoints = dataPoints;
	}

	public string Target { get; }
		
	[JsonPropertyName("datapoints")]
	public object[][] DataPoints { get; }
}