using Application;
using Application.Data;
using System.Globalization;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

const string path = "C:\\Users\\Acer\\source\\repos\\ETL\\Application\\Application\\sample-cab-data.csv";
const string dublicatepath = "C:\\Users\\Acer\\source\\repos\\ETL\\Application\\Application\\duplicates.csv";
const string connectionString = "Server=DESKTOP-S7D9M3G\\SQLEXPRESS;Database=ETL;Integrated Security=true;TrustServerCertificate=true;";
const string tableName = "Trips";

var dataContext = new EtlContext();

var csv = CSVDataProcessor.LoadCsvWithSelectedColumns(path);
DbDataProcessor.WriteToDatabase(connectionString, tableName, csv);

var dublicates = DbDataProcessor.RemoveDublicates(dataContext);
CSVDataProcessor.WriteTripsToCsv(dublicates,dublicatepath);