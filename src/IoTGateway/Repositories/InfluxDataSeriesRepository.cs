using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using IoTGateway.Models;
using IoTGateway.Services;

namespace IoTGateway.Repositories;

public class InfluxDataSeriesRepository: IDataSeriesRepository
{
    private readonly InfluxDBClient _client;
    const string Bucket = "test";
    const string Org = "fhlabor";

    public InfluxDataSeriesRepository(IConfiguration configuration)
    {
        var token = configuration.GetValue<string>("InfluxDB:Token");
        if (string.IsNullOrWhiteSpace(token))
        {
            throw MissingConfigurationException.Create("InfluxDB:Token");
        }

        _client = new InfluxDBClient("http://localhost:8086", token);
    }
    
    public void Dispose()
    {
        _client.Dispose();
    }

    public void Add(Reference reference, DateTime date, object? value)
    {
        var dataPoint = new DataPoint(reference, date, value);
        using (var writeApi = _client.GetWriteApi())
        {
            var point = PointData.Measurement(dataPoint.Reference.Name)
                .Field("value", dataPoint.Value)
                .Timestamp(dataPoint.DateTime, WritePrecision.Ms);

            writeApi.WritePoint(point, Bucket, Org);
        }
    }

    public async Task<IEnumerable<Reference>> GetReferencesAsync(int? skip = null, int? take = null)
    {
        var queryApi = _client.GetQueryApi();
        // Define a Flux query to list measurements in the specified bucket
        string flux = $"import \"influxdata/influxdb/schema\"\n" +
                       $"schema.measurements(bucket: \"{Bucket}\")";
        
        var queryResult = await queryApi.QueryAsync(flux, Org);

        // Extract the list of measurements from the query result
        var resultList = new List<Reference>();
        var measurements = queryResult.FirstOrDefault()?.Records;
        if (measurements != null)
        {
            foreach (var record in measurements)
            {
                var v = (string)record.Values["_value"];
                    
                resultList.Add(new Reference(v)); 
            }
        }
        
        return resultList;
    }

    public async Task<IEnumerable<DataPoint>> GetDataPointsAsync(Reference reference, DateTime startDateTime, DateTime endDateTime, int? skip = null, int? take = null)
    {
        var query = _client.GetQueryApi();
        var flux = $"from(bucket:\"{Bucket}\") " +
                   $"|> range(start: {startDateTime:o}, stop: {endDateTime:o}) " +
                   $"|> filter(fn: (r) => r[\"_measurement\"] == \"{reference.Name}\")";
        var tables = await query.QueryAsync(flux, Org);
        var points = tables.SelectMany(table =>
            table.Records.Select(record =>
                {
                    var t = record.GetTime().HasValue ? record.GetTime()!.Value.ToDateTimeUtc() : DateTime.MinValue;
                    var v = record.Values["_value"];
                    
                    return new DataPoint(reference, t, v);
                }
            ));
        
        return points;
    }
}