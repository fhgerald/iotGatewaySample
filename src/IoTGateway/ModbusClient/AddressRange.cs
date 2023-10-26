namespace IoTGateway.ModbusClient;

/// <summary>
/// Defines modbus address range
/// </summary>
public class AddressRange
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="startingAddress">First input register to be read</param>
    /// <param name="quantity">Number of input registers to be read</param>
    public AddressRange(int startingAddress, int quantity)
    {
        StartingAddress = startingAddress;
        Quantity = quantity;
    }

    /// <summary>
    /// Start address of the address range
    /// </summary>
    public int StartingAddress { get; }
    
    /// <summary>
    /// Amount of addresses in the address range to read/write
    /// </summary>
    public int Quantity { get; }

}