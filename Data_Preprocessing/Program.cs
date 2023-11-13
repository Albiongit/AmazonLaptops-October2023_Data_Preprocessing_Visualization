using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public class Program
{
    static void Main()
    {
        // Specify the path to dataset CSV file
        string csvFile = GetCsvFilePath("SharedData", "Data", "amazon_laptops.csv");

        // Read the CSV file and create a list of dynamic objects - take only first 10 records for testing
        List<dynamic> data = ReadCsv(csvFile);

        // Display data in records
        DisplayData(data);
    }

    static List<dynamic> ReadCsv(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            return csv.GetRecords<dynamic>().Take(10).ToList();
        }
    }

    static string GetCsvFilePath(params string[] paths)
    {
        string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

        return Path.GetFullPath(Path.Combine(solutionDirectory, Path.Combine(paths)));
    }

    static void DisplayData(List<dynamic> data)
    {
        foreach (var item in data)
        {
            Console.WriteLine("--------------- Record ---------------");

            foreach (var property in item)
            {
                Console.WriteLine($"{property.Key}: {property.Value}");
            }

            Console.WriteLine();
        }
    }
}