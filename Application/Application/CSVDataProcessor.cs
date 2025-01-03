using Microsoft.Data.SqlClient;
using System.Data;

namespace Application;

internal static class CSVDataProcessor
{
    private static StreamReader reader;

    internal static void OpenCSV(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("File does not exist");
        }
        reader = new StreamReader(File.OpenRead(path));
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line!.Split(',');
        }
        reader.Close();
    }
    internal static DataTable LoadCsvWithSelectedColumns(string filePath)
    {
        DataTable dataTable = new DataTable();

        dataTable.Columns.Add("tpep_pickup_datetime", typeof(DateTime));
        dataTable.Columns.Add("tpep_dropoff_datetime", typeof(DateTime));
        dataTable.Columns.Add("passenger_count", typeof(int));
        dataTable.Columns.Add("trip_distance", typeof(double));
        dataTable.Columns.Add("store_and_fwd_flag", typeof(string));
        dataTable.Columns.Add("PULocationID", typeof(int));
        dataTable.Columns.Add("DOLocationID", typeof(int));
        dataTable.Columns.Add("fare_amount", typeof(decimal));
        dataTable.Columns.Add("tip_amount", typeof(decimal));

        using (var reader = new StreamReader(filePath))
        {
            string[] headers = reader.ReadLine().Split(',');

            while (!reader.EndOfStream)
            {
                string[] row = reader.ReadLine().Split(',');
                DataRow dataRow = dataTable.NewRow();

                dataRow["tpep_pickup_datetime"] = DateTime.Parse(row[1]);
                dataRow["tpep_dropoff_datetime"] = DateTime.Parse(row[2]);
                dataRow["passenger_count"] = string.IsNullOrWhiteSpace(row[3]) ? 0 : int.Parse(row[3]);
                dataRow["trip_distance"] = double.Parse(row[4]);
                dataRow["store_and_fwd_flag"] = row[6] == "N" ? "No" : "Yes";
                dataRow["PULocationID"] = int.Parse(row[7]);
                dataRow["DOLocationID"] = int.Parse(row[8]);
                dataRow["fare_amount"] = decimal.Parse(row[10]);
                dataRow["tip_amount"] = decimal.Parse(row[13]);

                dataTable.Rows.Add(dataRow);
            }
        }

        return dataTable;
    }

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

    internal
}
