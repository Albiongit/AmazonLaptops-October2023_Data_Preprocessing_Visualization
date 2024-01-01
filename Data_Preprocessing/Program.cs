using Application.Extensions;
using Application.Helpers;
using Application.Services;
using SharedData.Mappers;
using SharedData.Models;

public class Program
{
    static void Main()
    {   
        // Specify the path to dataset CSV file
        string csvFile = CsvServiceHelper.GetCsvFilePath("SharedData", "Data", "amazon_laptops.csv");

        // Read the CSV file and create a list of amazon laptops - take only first 10 records for testing
        List<AmazonLaptopModel> data = CsvServiceHelper.ReadCsv<AmazonLaptopModel, AmazonLaptopModelMap>(csvFile);
        Console.WriteLine($"- Number of records in original dataset: {data.Count} -");

        // Include processing data service to use processing methods
        DataProcessingService processingService = new DataProcessingService();

        // Display data in records
        processingService.DisplayInitialData(data, "Original dataset model");

        // Count null values for each column for dataset with original model
        processingService.DisplayCountedNullValues(data, "Original dataset model");

        // Remove unnecessary columns and store the data in a processed model class
        var processedDataList = processingService.GetProcessedData(data);
        processingService.DisplayInitialData(processedDataList, "Preprocessed dataset model");

        // Check for duplicates, if any then remove
        processedDataList = processingService.RemoveDuplicates(processedDataList);

        // Count null values for each column for dataset with processed model
        processingService.DisplayCountedNullValues(processedDataList, "Preprocessed dataset model");

        // Process and fill null values
        var queryProcessedData = processedDataList.AsQueryable();

        // Get most repeated values in Ram and Hard disk columns and initialize null records with it
        var mostRepeatedValues = queryProcessedData.FindMostRepeatedValueForRamAndHardDisk(x => x.Ram, x => x.HardDisk);

        processingService.FillNullValues(processedDataList, "Ram", mostRepeatedValues!.Value.mostRepeatedRamValue!.Value);
        processingService.FillNullValues(processedDataList, "HardDisk", mostRepeatedValues!.Value.mostRepeatedHardDiskValue!.Value);

        // Get average value in Screen size column and intialize null records with it
        double screenSizeAverageValue = queryProcessedData.Select(x => x.ScreenSize).Average()!.Value;
        processingService.FillNullValues(processedDataList, "ScreenSize", screenSizeAverageValue);

        // Remove records where price is a null value
        processedDataList.RemoveAll(x => x.Price == null);

        // Count null values for each column for dataset with processed model
        processingService.DisplayCountedNullValues(processedDataList, "Preprocessed dataset model after null removal");

        Console.WriteLine($"- Number of records in preprocessed dataset: {processedDataList.Count} -");

        // Save new preprocessed list in a csv file
        string preprocessedCsvFile = CsvServiceHelper.GetCsvFilePath("SharedData", "Data", "amazon_laptops_preprocessed.csv");
        CsvServiceHelper.WriteToCsv(processedDataList, preprocessedCsvFile);
    }
}