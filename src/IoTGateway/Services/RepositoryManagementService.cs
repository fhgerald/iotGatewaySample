using IoTGateway.ModbusClient;
using IoTGateway.Models;
using IoTGateway.Repositories;
using NLog;
using ILogger = NLog.ILogger;

namespace IoTGateway.Services;

/// <summary>
/// This is a hosted service that initializes the repository service and adds a data series recorder to it.
/// </summary>
internal class RepositoryManagementService : IHostedService
{
    private readonly IMyModbusClient _myModbusClient;
    private readonly IDataSeriesRepository _repositoryService;
    private readonly List<DataSeriesRecorder<double?>> _recorders;
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    public RepositoryManagementService(IMyModbusClient myModbusClient, IDataSeriesRepository repositoryService)
    {
        _myModbusClient = myModbusClient;
        _repositoryService = repositoryService;
        _recorders = new List<DataSeriesRecorder<double?>>();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var time = new TimeSpan(0, 0, 10);

        // Connects to modbus server
        try
        {
            Logger.Info("Startup: Connecting to MODBUS server and configuring it.");

            _myModbusClient.Connect();
            _myModbusClient.AddReadInputRegisterPolling(new AddressRange(10, 10));

            Logger.Info("Startup: Initializing data series recorder.");
            var seriesRecorder = new DataSeriesRecorder<double?>(_repositoryService, time,
                () =>
                {
                    var value = _myModbusClient.GetInputRegisterValue(10 + 6);
                    if (value == null)
                    {
                        return null;
                    }

                    return Convert.ToDouble(value);
                }, new Reference("Power_W"));
            _recorders.Add(seriesRecorder);
            
            seriesRecorder = new DataSeriesRecorder<double?>(_repositoryService, time,
                () =>
                {
                    var value = _myModbusClient.GetInputRegisterValue(10 + 4);
                    if (value == null)
                    {
                        return null;
                    }

                    return Convert.ToDouble(value);
                }, new Reference("Voltage_cV"));
            _recorders.Add(seriesRecorder);

            // This call "starts" the production for simulation purposes
            Logger.Info("Startup: Starting production for simulation purposes.");
            _myModbusClient.WriteCoil(10, true);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            Logger.Info("Shutdown: Stopping recorders");
            foreach (var dataSeriesRecorder in _recorders)
            {
                dataSeriesRecorder.Dispose();
            }
            
            Logger.Info("Shutdown: Disconnecting from MODBUS server.");
            _myModbusClient.Disconnect();
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }

        return Task.CompletedTask;
    }
}