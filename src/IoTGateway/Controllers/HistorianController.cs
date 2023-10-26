using IoTGateway.Models;
using IoTGateway.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IoTGateway.Controllers;

/// <summary>
/// REST API for the historian.
/// </summary>
[ApiController]
[Route("[controller]")]
public class HistorianController : ControllerBase
{
    private readonly ILogger<HistorianController> _logger;
    private readonly IDataSeriesRepository _repository;

    public HistorianController(ILogger<HistorianController> logger, IDataSeriesRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    /// <summary>
    /// Get all references.
    /// </summary>
    /// <returns></returns>
    [HttpGet("references")]
    public async Task<IEnumerable<Reference>> GetReferences()
    {
        return await _repository.GetReferencesAsync();
    }
    
    /// <summary>
    /// Get data points for a reference by filter options
    /// </summary>
    /// <param name="reference"></param>
    /// <param name="seconds">Amount of seconds data points are returned</param>
    /// <returns></returns>
    [HttpGet("dataPoints/{reference}/{seconds}")]
    public async Task<IEnumerable<DataPoint>> GetDataPoints(string reference, int seconds)
    {
        var endDateTime = DateTime.UtcNow;
        var startDateTime = endDateTime.Subtract(new TimeSpan(0, 0, seconds));
        return await _repository.GetDataPointsAsync(reference, startDateTime, endDateTime);
    }
}