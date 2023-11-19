using Application.Converters;
using Application.Extensions;
using Application.Helpers;
using SharedData.Mappers;
using SharedData.Models;
using System.Reflection;

public class Program
{
    static void Main()
    {
        // Specify the path to dataset CSV file
        string csvFile = CsvServiceHelper.GetCsvFilePath("SharedData", "Data", "amazon_laptops.csv");

        // Read the CSV file and create a list of amazon laptops - take only first 10 records for testing
        List<AmazonLaptopModel> data = CsvServiceHelper.ReadCsv<AmazonLaptopModel, AmazonLaptopModelMap>(csvFile);
        Console.WriteLine("Number of records in original dataset: " + (data.Count - 1));

        // Display data in records
        DisplayInitialData(data);

        // Count null values for each column for dataset with original model
        DisplayCountedNullValues(data);

        // Remove unnecessary columns and store the data in a processed model class
        var processedDataList = GetProcessedData(data);
        DisplayInitialData(processedDataList);

        // Count null values for each column for dataset with processed model
        DisplayCountedNullValues(processedDataList);

        // Process and fill null values
        var queryProcessedData = processedDataList.AsQueryable();

        // Get most repeated values in Ram and Hard disk columns and initialize null records with it
        var mostRepeatedValues = queryProcessedData.FindMostRepeatedValueForRamAndHardDisk(x => x.Ram, x => x.HardDisk);

        FillNullValues(processedDataList, "Ram", mostRepeatedValues!.Value.mostRepeatedRamValue!.Value);
        FillNullValues(processedDataList, "HardDisk", mostRepeatedValues!.Value.mostRepeatedHardDiskValue!.Value);

        // Get average value in Screen size column and intialize null records with it
        double screenSizeAverageValue = queryProcessedData.Select(x => x.ScreenSize).Average()!.Value;
        FillNullValues(processedDataList, "ScreenSize", screenSizeAverageValue);

        // Remove records where price is a null value
        processedDataList.RemoveAll(x => x.Price == null);

        // Count null values for each column for dataset with processed model
        DisplayCountedNullValues(processedDataList);
    }

    static void DisplayInitialData<T>(List<T> data)
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

    static List<AmazonLaptopProcessedModel> GetProcessedData(List<AmazonLaptopModel> data)
    {
        List<AmazonLaptopProcessedModel> processedDataList = data
            .Select(x => new AmazonLaptopProcessedModel
            {
                Brand = x.Brand,
                Ram = TypeParser.ParseHardDiskOrRam(x.Ram),
                ScreenSize = TypeParser.ParseToDouble(x.ScreenSize),
                HardDisk = TypeParser.ParseHardDiskOrRam(x.HardDisk),
                Price = TypeParser.ParseToDouble(x.Price)
            }).ToList();

        return processedDataList;
    }

    static void FillNullValues(List<AmazonLaptopProcessedModel> data, string propertyName, object valueToFill)
    {
        foreach (var item in data)
        {
            var property = typeof(AmazonLaptopProcessedModel).GetProperty(propertyName);
            if (property != null)
            {
                var currentValue = property.GetValue(item);
                if (currentValue == null)
                {
                    property.SetValue(item, valueToFill);
                }
            }
        }
    }
}