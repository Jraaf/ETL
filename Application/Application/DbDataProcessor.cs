using Application.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Application;

internal class DbDataProcessor
{
    internal static void WriteToDatabase(string connectionString, string tableName, DataTable dataTable)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.ColumnMappings.Add("tpep_pickup_datetime", "tpep_pickup_datetime");
                bulkCopy.ColumnMappings.Add("tpep_dropoff_datetime", "tpep_dropoff_datetime");
                bulkCopy.ColumnMappings.Add("passenger_count", "passenger_count");
                bulkCopy.ColumnMappings.Add("trip_distance", "trip_distance");
                bulkCopy.ColumnMappings.Add("store_and_fwd_flag", "store_and_fwd_flag");
                bulkCopy.ColumnMappings.Add("PULocationID", "PULocationID");
                bulkCopy.ColumnMappings.Add("DOLocationID", "DOLocationID");
                bulkCopy.ColumnMappings.Add("fare_amount", "fare_amount");
                bulkCopy.ColumnMappings.Add("tip_amount", "tip_amount");

                bulkCopy.WriteToServer(dataTable);
            }
        }

        Console.WriteLine("Data imported successfully!");
    }
    /// <summary>
    /// Removes all the dublicates
    /// </summary>
    /// <param name="context"></param>
    /// <returns>All the removed dublicates</returns>
    internal static List<Trip> RemoveDublicates(EtlContext context)
    {
        var distinctRecords = context.Trips
            .GroupBy(x => new
            {
                x.TpepPickupDatetime,
                x.TpepDropoffDatetime,
                x.PassengerCount
            })
            .Select(g => g.First().Id)
            .ToList();
        
        var duplicateRecords = context.Trips
            .Where(x => !distinctRecords.Contains(x.Id))
            .ToList();

        context.Trips.RemoveRange(duplicateRecords);
        context.SaveChanges();
        return duplicateRecords;
    }
    internal static void HighestAverageTip(EtlContext context)
    {
        var highestAvgTip = context.Trips
               .GroupBy(x => x.PulocationId)
               .Select(g => new
               {
                   PulocationId = g.Key,
                   AvgTipAmount = g.Average(x => x.TipAmount)
               })
               .OrderByDescending(x => x.AvgTipAmount)
               .FirstOrDefault();

        Console.WriteLine($"Highest Avg Tip Amount: PULocationID = {highestAvgTip?.PulocationId}, AvgTipAmount = {highestAvgTip?.AvgTipAmount}");
    }

    internal static void Top100LongestFairsInDistance(EtlContext context)
    {
        var longestFaresByDistance = context.Trips
               .OrderByDescending(x => x.TripDistance)
               .Take(100)
               .ToList();

        Console.WriteLine("Top 100 longest fares by trip_distance:");
        foreach (var fare in longestFaresByDistance)
        {
            Console.WriteLine($"Trip Distance: {fare.TripDistance}, PULocationID: {fare.PulocationId}, DOLocationID: {fare.DolocationId}");
        }
    }

    internal static void Top100LongestFairsInTime(EtlContext context)
    {
        var longestFaresByTime = context.Trips
                .Select(x => new
                {
                    x.PulocationId,
                    x.DolocationId,
                    x.TpepPickupDatetime,
                    x.TpepDropoffDatetime,
                    TripDuration = x.TpepDropoffDatetime.Value.AddDays(-x.TpepPickupDatetime.Value.Day)
                })
                .OrderByDescending(x => x.TripDuration)
                .Take(100)
                .ToList();

        Console.WriteLine("Top 100 longest fares by trip duration:");
        foreach (var fare in longestFaresByTime)
        {
            Console.WriteLine($"Trip Duration: {fare.TripDuration} minutes, PULocationID: {fare.PulocationId}, DOLocationID: {fare.DolocationId}");
        }
    }
    internal static void SearchByLocation(EtlContext context, int pickUpLocationId)
    {
        var faresByPULocationID = context.Trips
               .Where(x => x.PulocationId == pickUpLocationId)
               .ToList();

        Console.WriteLine($"Fares for PULocationID = {pickUpLocationId}:");
        foreach (var fare in faresByPULocationID)
        {
            Console.WriteLine($"\tTrip Distance: {fare.TripDistance}, Tip Amount: {fare.TipAmount}");
        }
    }
}
