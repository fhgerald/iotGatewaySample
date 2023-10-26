namespace IoTGateway.Services;

public class MissingConfigurationException : Exception
{
    private MissingConfigurationException()
    {
    }

    private MissingConfigurationException(string message) : base(message)
    {
    }

    private MissingConfigurationException(string message, Exception inner) : base(message, inner)
    {
    }

    public static Exception Create(string influxdbToken)
    {
        return new MissingConfigurationException($"Missing configuration for {influxdbToken}");
    }
}