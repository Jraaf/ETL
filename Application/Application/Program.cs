using Application;
using Application.Data;
using System.Globalization;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

const string path = "C:\\Users\\Acer\\source\\repos\\ETL\\Application\\Application\\sample-cab-data.csv";
const string dublicatepath = "C:\\Users\\Acer\\source\\repos\\ETL\\Application\\Application\\duplicates.csv";
const string connectionString = "Server=DESKTOP-S7D9M3G\\SQLEXPRESS;Database=ETL;Integrated Security=true;TrustServerCertificate=true;";
const string tableName = "Trips";

const string border = "\n\t----------\n";

var dataContext = new EtlContext();
string input = "";

while (input.ToLower() != "exit")
{
    Console.Write("Available options:\n" +
        "(1)\t- Find out which `PULocationId` (Pick-up location ID) has the highest tip_amount on average.\n"
        + "(2)\t- Find the top 100 longest fares in terms of `trip_distance`\n"
        + "(3)\t- Find the top 100 longest fares in terms of time spent traveling.'\n"
        + "(4)\t- Search, where part of the conditions is `PULocationId`\n"
        + "(5)\t- Load Data all over again \n"
        + "'exit' to leave\n"
        + ">>");
    input = Console.ReadLine();
    switch (input)
    {
        case "1":
            Console.WriteLine(border);
            DbDataProcessor.HighestAverageTip(dataContext);
            Console.WriteLine(border);
            break;
        case "2":
            Console.WriteLine(border);
            DbDataProcessor.Top100LongestFairsInDistance(dataContext);
            Console.WriteLine(border);
            break;
        case "3":
            Console.WriteLine(border);
            DbDataProcessor.Top100LongestFairsInTime(dataContext);
            Console.WriteLine(border);
            break;
        case "4":

            Console.Write("Write the PULocationId>>");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                break;
            }
            Console.WriteLine(border);
            DbDataProcessor.SearchByLocation(dataContext, id);
            Console.WriteLine(border);
            break;
        case "exit":
            return;
        default:
            break;
    }
}
var csv = CSVDataProcessor.LoadCsvWithSelectedColumns(path);
DbDataProcessor.WriteToDatabase(connectionString, tableName, csv);

var dublicates = DbDataProcessor.RemoveDublicates(dataContext);
CSVDataProcessor.WriteTripsToCsv(dublicates, dublicatepath);