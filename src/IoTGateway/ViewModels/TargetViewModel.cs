namespace IoTGateway.ViewModels;

// ReSharper disable once ClassNeverInstantiated.Global
public class TargetViewModel
{
	public TargetViewModel(string target)
	{
		Target = target;
	}

	public string Target { get; }
}