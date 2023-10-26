namespace IoTGateway.ModbusClient;

/// <summary>
/// Interface for the modbus client
/// </summary>
public interface IMyModbusClient
{
    /// <summary>
    /// Time in ms between two polls
    /// </summary>
    int PollingTimeMs { get; set; }
    
    /// <summary>
    /// Time in ms between two reconnects
    /// </summary>
    int ReconnectTimeMs { get; set; }
    
    /// <summary>
    /// When true, the client tries to reconnect automatically
    /// </summary>
    bool IsAutoReconnectEnabled { get; set; }
    
    /// <summary>
    /// IP address of the modbus server
    /// </summary>
    string IpAddress { get; set; }
    
    /// <summary>
    /// Port of the modbus server
    /// </summary>
    int Port { get; set; }
    
    /// <summary>
    /// Connects to the modbus server
    /// </summary>
    void Connect();
    
    /// <summary>
    /// Disconnects from the modbus server
    /// </summary>
    void Disconnect();

    /// <summary>
    /// Defines an address range that gets polled as MODBUS InputRegister 
    /// </summary>
    /// <param name="addressRange">The address range polled for</param>
    void AddReadInputRegisterPolling(AddressRange addressRange);

    /// <summary>
    /// Returns the value of an address
    /// </summary>
    /// <param name="address">The address (needs to be defined before for polling)</param>
    /// <returns></returns>
    int? GetInputRegisterValue(int address);

    void WriteCoil(int address, bool value);
    void Dispose();
}