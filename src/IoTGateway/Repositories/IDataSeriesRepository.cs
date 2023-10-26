using IoTGateway.Models;

namespace IoTGateway.Repositories
{
    /// <summary>
    /// Generic interface for a data series repository.
    /// </summary>
    public interface IDataSeriesRepository : IDisposable
    {
        /// <summary>
        /// Add a data point to the repository.
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="date"></param>
        /// <param name="value"></param>
        void Add(Reference reference, DateTime date, object? value);

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <param name="skip">Amount of references to skip</param>
        /// <param name="take">Amount of references to take</param>
        /// <returns></returns>
        Task<IEnumerable<Reference>> GetReferencesAsync(int? skip = null, int? take = null);
        
        /// <summary>
        /// Get data points for a reference by filter options
        /// </summary>
        /// <param name="reference">Reference of the data point</param>
        /// <param name="startDateTime">Start date time of the data points</param>
        /// <param name="endDateTime">End date time of the data points</param>
        /// <param name="skip">Amount of data points to skip</param>
        /// <param name="take">Amount of data points to take</param>
        /// <returns></returns>
        Task<IEnumerable<DataPoint>> GetDataPointsAsync(Reference reference, DateTime startDateTime, DateTime endDateTime, int? skip = null, int? take = null);
    }
}