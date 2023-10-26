namespace IoTGateway.Models;

/// <summary>
/// Represents a data point with reference, date time and value.
/// </summary>
public struct DataPoint
{
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="reference"></param>
	/// <param name="dateTime"></param>
	/// <param name="value"></param>
	public DataPoint(Reference reference, DateTime dateTime, object? value)
	{
		Reference = reference;
		DateTime = dateTime;
		Value = value;
	}

	/// <summary>
	/// Date and time of the data point
	/// </summary>
	public DateTime DateTime { get; }
	
	/// <summary>
	/// Value of the data point
	/// </summary>
	public object? Value { get; }
	
	/// <summary>
	/// Reference of the data point
	/// </summary>
	public Reference Reference { get;  }
}