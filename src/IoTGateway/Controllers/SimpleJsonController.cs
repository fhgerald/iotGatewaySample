using IoTGateway.Models;
using IoTGateway.Repositories;
using IoTGateway.ViewModels;
using Microsoft.AspNetCore.Mvc;
using IoTGateway.DataHandling;

namespace IoTGateway.Controllers;

/// <summary>
/// Implements Grafana Simple JSON interface, see https://grafana.com/grafana/plugins/grafana-simple-json-datasource/
/// </summary>
[ApiController]
[Route("")]
public class SimpleJsonController : ControllerBase
{
    private readonly ILogger<SimpleJsonController> _logger;
    private readonly IDataSeriesRepository _dataSeriesRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="dataSeriesRepository">Service that provides access to historical data</param>
    public SimpleJsonController(ILogger<SimpleJsonController> logger, IDataSeriesRepository dataSeriesRepository)
    {
        _logger = logger;
        _dataSeriesRepository = dataSeriesRepository;
    }

    /// <summary>
    /// Return 200 ok. Used for "Test connection" on the datasource config page.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Get() => Ok();

    /// <summary>
    /// Used by the find metric options on the query tab in panels.
    /// </summary>
    /// <returns></returns>
    [HttpPost("search")]
    public async Task<IActionResult> Search()
    {
        var references = await _dataSeriesRepository.GetReferencesAsync();
        
        return Ok(references.Select(x=> 
            new ReferenceViewModel{ Text = $"{x.Name}", Value = x.Name}));
    }

    /// <summary>
    /// Returns metrics based on input.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost("query")]
    public async Task<IActionResult> Query([FromBody] QueryViewModel query)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var smoothingWindow = TimeSpan.FromTicks(5 * TimeSpan.TicksPerMinute);
        var halfSmoothingWindow = 0.5 * smoothingWindow;

        var dataFrom = query.Range.From - halfSmoothingWindow;
        var dataTo = query.Range.To + halfSmoothingWindow;

        var samplingInterval = new TimeSpan(query.IntervalMs * TimeSpan.TicksPerMillisecond);
        
        // Ensure that there is a valid target
        if (query.Targets.Any(t => string.IsNullOrWhiteSpace(t.Target)))
        {
            return Ok(new TimeSeriesViewModel[]{});
        }

        // Ensure that there is a valid target
        if (query.Targets.Any(t => string.IsNullOrWhiteSpace(t.Target)))
        {
            return Ok(new TimeSeriesViewModel[]{});
        }

        var resultList = new List<TimeSeriesViewModel>();
        foreach (var target in query.Targets)
        {
            var reference = new Reference(target.Target);

            var dataPoints = await _dataSeriesRepository.GetDataPointsAsync(reference, dataFrom, dataTo);
            
            // This code is needed to adapt the data points to the Grafana format
            var r = dataPoints.FilterDataPoints(reference.Name, query.Range.From, query.Range.To, 
                    samplingInterval, smoothingWindow)
                .Select(x => new[] { x.Value, x.DateTime.ToUnixEpochInMilliSecondsTime() }).ToArray();
            
            resultList.Add(new TimeSeriesViewModel(reference.Name, r));
        }

        return Ok(resultList);
    }

    /// <summary>
    /// Return annotations. In your sample unused.
    /// </summary>
    /// <returns></returns>
    [HttpPost("annotations")]
    public IActionResult GetAnnotations()
    {
        return Ok();
    }
}
