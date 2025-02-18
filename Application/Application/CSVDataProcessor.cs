﻿using Application.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Data;
using System.Text;

namespace Application;

internal static class CSVDataProcessor
{
    private static StreamReader reader;
    private static string header;
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

        header = "tpep_pickup_datetime,tpep_dropoff_datetime,passenger_count,trip_distance,store_and_fwd_flag,PULocationID,DOLocationID,fare_amount,tip_amount";

        using (var reader = new StreamReader(filePath))
        {
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string[] row = reader.ReadLine().Split(',');
                DataRow dataRow = dataTable.NewRow();

                dataRow["tpep_pickup_datetime"] = DateTime.Parse(row[1].Trim()).ToUniversalTime();
                dataRow["tpep_dropoff_datetime"] = DateTime.Parse(row[2].Trim()).ToUniversalTime();
                dataRow["passenger_count"] = string.IsNullOrWhiteSpace(row[3].Trim()) ? 0 : int.Parse(row[3].Trim());
                dataRow["trip_distance"] = double.Parse(row[4].Trim());
                dataRow["store_and_fwd_flag"] = row[6] == "N" ? "No" : "Yes";
                dataRow["PULocationID"] = int.Parse(row[7].Trim());
                dataRow["DOLocationID"] = int.Parse(row[8].Trim());
                dataRow["fare_amount"] = decimal.Parse(row[10].Trim());
                dataRow["tip_amount"] = decimal.Parse(row[13].Trim());

                dataTable.Rows.Add(dataRow);
            }
        }

        return dataTable;
    }
    internal static void WriteTripsToCsv(List<Trip> duplicates, string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(header);
            // Write duplicate records
            foreach (var duplicate in duplicates)
            {
                writer.WriteLine(duplicate.ToString());
            }
        }
    }

}
