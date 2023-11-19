using Application.Converters;
using SharedData.Models;
using System.Reflection;

namespace Application.Services;

public class DataProcessingService
{
    public void DisplayInitialData<T>(List<T> data, string title)
    {
        int i = 1;
        Console.WriteLine($"\n------------------ Some records of Amazon laptops - {title} ------------------\n");

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

        Console.WriteLine($"------------------------------------------------------------------------------------\n");
    }

    public void DisplayCountedNullValues<T>(List<T> data, string title)
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

        Console.WriteLine($"------------------ Number of null values for each column - {title} ------------------");
        foreach (var kvp in nullCounts)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value} null values");
        }

        Console.WriteLine();
    }

    public List<AmazonLaptopProcessedModel> GetProcessedData(List<AmazonLaptopModel> data)
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

    public void FillNullValues(List<AmazonLaptopProcessedModel> data, string propertyName, object valueToFill)
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
    public List<AmazonLaptopProcessedModel> RemoveDuplicates(List<AmazonLaptopProcessedModel> list)
    {
        HashSet<AmazonLaptopProcessedModel> set = new();

        bool duplicatesExists = !list.All(set.Add);

        if (duplicatesExists)
        {
            return new HashSet<AmazonLaptopProcessedModel>(list).ToList();
        }
        else
        {
            Console.WriteLine("Dataset does not contain any duplicates\n");

            return list;
        }
    }
}
