﻿using Application.Converters;
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

    public List<AmazonLaptopProcessedModel> GetProcessedData(List<AmazonLaptopProcessedModel> data)
    {
        List<AmazonLaptopProcessedModel> processedDataList = data
            .Select(x => new AmazonLaptopProcessedModel
            {
                Brand = x.Brand,
                Ram = x.Ram,
                ScreenSize = x.ScreenSize,
                HardDisk = x.HardDisk,
                Price = x.Price
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

    public double CalculateStandardDeviation(IEnumerable<double> listOfValues)
    {
        double average = listOfValues.Average();

        double sumOfSquares = listOfValues.Sum(val => Math.Pow(val - average, 2));

        return Math.Sqrt(sumOfSquares / listOfValues.Count());
    }

    public List<int> FindOutliersViaZScore(List<double> zScores, double threshold)
    {
        List<int> outlierIndices = new List<int>();

        for (int i = 0; i < zScores.Count; i++)
        {
            if (Math.Abs(zScores[i]) > threshold)
            {
                outlierIndices.Add(i);
            }
        }

        return outlierIndices;
    }

    public double[,] CalculateCorrelationMatrix(List<double> values1, List<double> values2, List<double> values3, List<double> values4)
    {
        List<List<double>> listOfColumnValues = new List<List<double>>()
        { 
            values1, 
            values2, 
            values3, 
            values4
        };

        int numOfColumn = listOfColumnValues.Count;

        double[,] correlationMatrix = new double[numOfColumn, numOfColumn];

        // Populate the correlation matrix
        for (int i = 0; i < numOfColumn; i++)
        {
            for (int j = 0; j < numOfColumn; j++)
            {
                double correlation = CalculateCorrelation(listOfColumnValues[i], listOfColumnValues[j]);
                correlationMatrix[i, j] = correlation;
            }
        }

        return correlationMatrix;
    }

    private double CalculateCorrelation(List<double> x, List<double> y)
    {
        double stdDevOfX = CalculateStandardDeviation(x);
        double stdDevOfY = CalculateStandardDeviation(y);

        double covarianceOfXY = CalculateCovariance(x, y);

        return covarianceOfXY / (stdDevOfX * stdDevOfY);
    }

    private static double CalculateCovariance(List<double> x, List<double> y)
    {
        double meanX = x.Average();
        double meanY = y.Average();

        double covariance = x.Zip(y, (xi, yi) => (xi - meanX) * (yi - meanY)).Sum() / x.Count;

        return covariance;
    }
}
