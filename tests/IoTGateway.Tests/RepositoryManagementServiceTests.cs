using EasyModbus.Exceptions;
using FakeItEasy;
using IoTGateway.ModbusClient;
using IoTGateway.Repositories;
using IoTGateway.Services;

namespace IoTGateway.Tests;

public class RepositoryManagementServiceTests
{
    [Fact]
    public async Task StartAsync_ConnectionFailed_Exception()
    {
        var modbusClientFake = A.Fake<IMyModbusClient>();
        A.CallTo(() => modbusClientFake.Connect()).Throws<ModbusException>();

        var dataSeriesRepositoryFake = A.Fake<IDataSeriesRepository>();

        RepositoryManagementService repositoryManagementService = new(modbusClientFake, dataSeriesRepositoryFake);
        await Assert.ThrowsAsync<ModbusException>(() => repositoryManagementService.StartAsync(new CancellationToken()));
    }
}