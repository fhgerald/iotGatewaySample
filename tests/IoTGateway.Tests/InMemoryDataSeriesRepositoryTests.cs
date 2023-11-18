using IoTGateway.Models;
using IoTGateway.Repositories;

namespace IoTGateway.Tests;

public class InMemoryDataSeriesRepositoryTests
{
    [Fact]
    public void GetDataPointsAsync_OK()
    {
        Reference reference = new("demo");
        InMemoryDataSeriesRepository dataSeriesRepository = new();
        dataSeriesRepository.Add(reference, DateTime.Parse("2023-10-04T05:30:35Z"), 1);
        dataSeriesRepository.Add(reference, DateTime.Parse("2023-10-04T05:31:35Z"), 2);
        dataSeriesRepository.Add(reference, DateTime.Parse("2023-10-04T05:32:35Z"), 3);
        dataSeriesRepository.Add(reference, DateTime.Parse("2023-10-04T05:33:35Z"), 4);
        
        var dataPoints = dataSeriesRepository.GetDataPointsAsync(reference,
            DateTime.Parse("2023-10-04T05:30:35Z"), DateTime.Parse("2023-10-04T05:32:35Z")).Result;

        var enumerable = dataPoints as DataPoint[] ?? dataPoints.ToArray();
        Assert.Equal(3, enumerable.Length);
        Assert.Equal( DateTime.Parse("2023-10-04T05:30:35Z"), enumerable[0].DateTime);
        Assert.Equal( 1, enumerable[0].Value);
        Assert.Equal( DateTime.Parse("2023-10-04T05:31:35Z"), enumerable[1].DateTime);
        Assert.Equal( 2, enumerable[1].Value);
        Assert.Equal( DateTime.Parse("2023-10-04T05:32:35Z"), enumerable[2].DateTime);
        Assert.Equal( 3, enumerable[2].Value);
    } 
}