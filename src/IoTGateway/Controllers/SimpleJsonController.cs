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

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    public SimpleJsonController(ILogger<SimpleJsonController> logger)
    {
        _logger = logger;
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
        // TODO: return references
        return Ok(new List<ReferenceViewModel>());
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
        //foreach (var target in query.Targets)
        //{
            // TODO: Create a reference from the target
            // TODO: Get data points from the repository
        
        
            // This code is needed to adapt the data points to the Grafana format - that means wie have to 
            // build a list of data points adapted to the sampling interval and smoothing window
            // var r = dataPoints.FilterDataPoints(reference.Name, query.Range.From, query.Range.To, 
            //         samplingInterval, smoothingWindow)
            //     .Select(x => new[] { x.Value, x.DateTime.ToUnixEpochInMilliSecondsTime() }).ToArray();
            
       // }

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
