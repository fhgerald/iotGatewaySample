using IoTGateway.ModbusClient;
using Xunit.Abstractions;

namespace IoTGateway.Tests;

public class AddressRangeTests 
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AddressRangeTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public void EnsureMapping_OK()
    {
        var addressRange = new AddressRange(3, 5);
        Assert.Equal(3, addressRange.StartingAddress);
        Assert.Equal(5, addressRange.Quantity);
    }
    
    [Theory]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(int.MaxValue, 2)]
    public void EnsureMapping_DifferentParameters_OK(int startingAddress, int quantity)
    {
        var addressRange = new AddressRange(startingAddress, quantity);
        Assert.Equal(startingAddress, addressRange.StartingAddress);
        Assert.Equal(quantity, addressRange.Quantity);
        _testOutputHelper.WriteLine("this is message to the log of unit test");
    }
   
}