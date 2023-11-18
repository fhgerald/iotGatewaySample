using FakeItEasy;
using IoTGateway.ModbusClient;
using Microsoft.Extensions.DependencyInjection;

namespace IoTGateway.Tests.Fixtures;

public class ServiceCollectionFixture
{
    public ServiceCollection Services { get; }

    public ServiceCollectionFixture()
    {
        Services = new ServiceCollection();
      
        var modbusClientFake = A.Fake<IMyModbusClient>();
        A.CallTo(() => modbusClientFake.GetInputRegisterValue(A<int>._)).Returns(5);
        Services.AddSingleton(modbusClientFake);
    }
}