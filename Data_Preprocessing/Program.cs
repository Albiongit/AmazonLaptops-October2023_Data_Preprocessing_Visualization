using CsvHelper;
using CsvHelper.Configuration;
using SharedData.Mappers;
using SharedData.Models;
using System.Globalization;
using System.Reflection;

public class Program
{
    static void Main()
    {
        // Specify the path to dataset CSV file
        string csvFile = GetCsvFilePath("SharedData", "Data", "amazon_laptops.csv");

        // Read the CSV file and create a list of amazon laptops - take only first 10 records for testing
        List<AmazonLaptopModel> data = ReadCsv<AmazonLaptopModel, AmazonLaptopModelMap>(csvFile);
        Console.WriteLine("Number of records in original dataset: " + (data.Count - 1));

        // Display data in records
        DisplayInitialData(data);

        // Count null values for each column
        DisplayCountedNullValues(data);
    }

    static List<T> ReadCsv<T, TMap>(string filePath) where TMap : ClassMap<T>
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.Context.RegisterClassMap<TMap>();
            return csv.GetRecords<T>().ToList();
        }
    }

    static string GetCsvFilePath(params string[] paths)
    {
        string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

        return Path.GetFullPath(Path.Combine(solutionDirectory, Path.Combine(paths)));
    }

    static void DisplayInitialData(List<AmazonLaptopModel> data)
    {
        int i = 1;
        Console.WriteLine("\n------------------ Some records of Amazon laptops ------------------\n");

        foreach (var item in data.Take(10))
        {
            Console.WriteLine($"--------------- Record {i} ---------------");

            Type type = item.GetType();

            foreach (PropertyInfo property in type.GetProperties())
            {
                Console.WriteLine($"{property.Name}: {property.GetValue(item)}");
            }

            i++;
            Console.WriteLine();
        }
    }

    static void DisplayCountedNullValues<T>(List<T> data)
    {
        var nullCounts = new Dictionary<string, int>();

        // Get the properties of the class
        var properties = typeof(T).GetProperties();

        // Initialize counts to zero
        foreach (var property in properties)
        {
            nullCounts[property.Name] = 0;
        }

        // Count null values
        foreach (var item in data)
        {
            foreach (var property in properties)
            {
                var value = property.GetValue(item);
                if (value == null || value == "")
                {
                    nullCounts[property.Name]++;
                }
            }
        }

        Console.WriteLine("------------------ Number of null values for each column ------------------");
        foreach (var kvp in nullCounts)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value} null values");
        }

        Console.WriteLine();
    }
}