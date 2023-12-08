namespace IoTGateway.ViewModels;

public struct DateTimeRangeViewModel
{
	public DateTimeRangeViewModel(DateTime from, DateTime to)
	{
		From = from;
		To = to;
	}

	public DateTime From { get; set; }
	public DateTime To { get; set; }
}