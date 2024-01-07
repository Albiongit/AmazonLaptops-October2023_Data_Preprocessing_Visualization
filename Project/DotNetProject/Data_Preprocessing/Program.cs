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
        List<AmazonLaptopProcessedModel> processedDataList = processingService.GetProcessedData(data);
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

        Console.WriteLine($"- Number of records in preprocessed dataset: {processedDataList.Count} -\n");

        // Save new preprocessed list in a csv file -- Already saved -- will be commented until the end of project
        //string preprocessedCsvFile = CsvServiceHelper.GetCsvFilePath("SharedData", "Data", "amazon_laptops_preprocessed.csv");
        //CsvServiceHelper.WriteToCsv(processedDataList, preprocessedCsvFile);

        // Define outliers and anomalies via z-score method and remove them -- Second phase of the project

        Console.WriteLine("- Define outliers and anomalies by calculating mean, standard deviation and z-score for each column in preprocessed dataset -\n");

        // Calculate means and standard deviations for RAM, Hard Disk, Screen Size and Price
        double meanRam = processedDataList.Average(x => x.Ram.GetValueOrDefault(0.0));
        double stdDevRam = processingService.CalculateStandardDeviation(processedDataList.Select(x => x.Ram.GetValueOrDefault(0.0)));
        Console.WriteLine("RAM mean: {0}", meanRam);
        Console.WriteLine("RAM standard deviation: {0}\n", stdDevRam);

        double meanHardDisk = processedDataList.Average(x => x.HardDisk.GetValueOrDefault(0.0));
        double stdDevHardDisk = processingService.CalculateStandardDeviation(processedDataList.Select(x => x.HardDisk.GetValueOrDefault(0.0)));
        Console.WriteLine("Hard Disk mean: {0}", meanHardDisk);
        Console.WriteLine("Hard Disk standard deviation: {0}\n", stdDevHardDisk);

        double meanScreenSize = processedDataList.Average(x => x.ScreenSize.GetValueOrDefault(0.0));
        double stdDevScreenSize = processingService.CalculateStandardDeviation(processedDataList.Select(x => x.ScreenSize.GetValueOrDefault(0.0)));
        Console.WriteLine("Screen Size mean: {0}", meanScreenSize);
        Console.WriteLine("Screen Size standard deviation: {0}\n", stdDevScreenSize);

        double meanPrice = processedDataList.Average(x => x.Price.GetValueOrDefault(0.0));
        double stdDevPrice = processingService.CalculateStandardDeviation(processedDataList.Select(x => x.Price.GetValueOrDefault(0.0)));
        Console.WriteLine("Price mean: {0}", meanPrice);
        Console.WriteLine("Price standard deviation: {0}\n", stdDevPrice);

        // Calculate z-scores for RAM, Hard Disk, Screen Size and Price
        List<double> zScoresRAM = processedDataList.Select(x => (x.Ram.GetValueOrDefault(0.0) - meanRam) / stdDevRam).ToList();
        List<double> zScoresHardDisk = processedDataList.Select(x => (x.HardDisk.GetValueOrDefault(0.0) - meanHardDisk) / stdDevHardDisk).ToList();
        List<double> zScoresScreenSize = processedDataList.Select(x => (x.ScreenSize.GetValueOrDefault(0.0) - meanScreenSize) / stdDevScreenSize).ToList();
        List<double> zScoresPrice = processedDataList.Select(x => (x.Price.GetValueOrDefault(0.0) - meanPrice) / stdDevPrice).ToList();
        
        // Set a threshold for outlier detection
        double ramThreshold = 0.8;  // Experimentally tested
        double hardDiskThreshold = 0.4; // Experimentally tested
        double screenSizeThreshold = 3.0; // Experimentally tested
        double priceThreshold = 1.25; // Experimentally tested

        // Identify outliers
        List<int> outlierIndicesRAM = processingService.FindOutliersViaZScore(zScoresRAM, ramThreshold);
        List<int> outlierIndicesHardDisk = processingService.FindOutliersViaZScore(zScoresHardDisk, hardDiskThreshold);
        List<int> outlierIndicesScreenSize = processingService.FindOutliersViaZScore(zScoresScreenSize, screenSizeThreshold);
        List<int> outlierIndicesPrice = processingService.FindOutliersViaZScore(zScoresPrice, priceThreshold);

        // Show outlier indexes
        Console.WriteLine("Outliers in RAM(indexes): {0}.", (outlierIndicesRAM.Any() ? string.Join(", ", outlierIndicesRAM) : "Empty"));

        Console.WriteLine("Outliers in HardDisk(indexes): {0}.", (outlierIndicesHardDisk.Any() ? string.Join(", ", outlierIndicesHardDisk) : "Empty"));

        Console.WriteLine("Outliers in ScreenSize(indexes): {0}.", (outlierIndicesScreenSize.Any() ? string.Join(", ", outlierIndicesScreenSize) : "Empty"));

        Console.WriteLine("Outliers in Price(indexes): {0}.\n", (outlierIndicesPrice.Any() ? string.Join(", ", outlierIndicesPrice) : "Empty"));

        // Union all outlier indices and remove them from preprocessed dataset
        List<int> allOutlierIndices = outlierIndicesRAM
            .Union(outlierIndicesHardDisk)
            .Union(outlierIndicesScreenSize)
            .Union(outlierIndicesPrice)
            .ToList();

        // Remove outliers and anomalies
        foreach (int index in allOutlierIndices.OrderByDescending(i => i))
        {
            processedDataList.RemoveAt(index);
        }

        Console.WriteLine($"- Number of records in preprocessed dataset after outliers and anomalies removal: {processedDataList.Count} -\n");

        // Save new processed list after outliers and anomalies removal in a csv file 
        //string processedCsvFile = CsvServiceHelper.GetCsvFilePath("SharedData", "Data", "amazon_laptops_processed.csv");
        //CsvServiceHelper.WriteToCsv(processedDataList, processedCsvFile);

        // Extract numerical attributes for correlation matrix
        List<double> ramValues = processedDataList.Select(x => x.Ram.GetValueOrDefault(0.0)).ToList();
        List<double> hardDiskValues = processedDataList.Select(x => x.HardDisk.GetValueOrDefault(0.0)).ToList();
        List<double> screenSizeValues = processedDataList.Select(x => x.ScreenSize.GetValueOrDefault(0.0)).ToList();
        List<double> priceValues = processedDataList.Select(x => x.Price.GetValueOrDefault(0.0)).ToList();

        // Calculate the correlation matrix
        double[,] correlationMatrix = processingService.CalculateCorrelationMatrix(ramValues, hardDiskValues, screenSizeValues, priceValues);
        
        // Call service for displaying correlation matrix and general statistics of numerical columns
        CommonService commonService = new CommonService();

        // Display the correlation matrix
        commonService.DisplayCorrelationMatrix(correlationMatrix);

        // General statistics for all numerical columns
        Console.WriteLine("- Ram general statistics -");
        double finalRamMeanValue = CommonHelper.CalculateMean(ramValues);
        double finalStdDevRamValue = processingService.CalculateStandardDeviation(ramValues);
        double finalRamMedianValue = CommonHelper.CalculateMedian(ramValues);
        double finalRamMinValue = CommonHelper.CalculateMin(ramValues);
        double finalRamMaxValue = CommonHelper.CalculateMax(ramValues);
        List<double> finalRamModeValue = CommonHelper.CalculateMode(ramValues);

        commonService.DisplayGeneralStatistics(finalRamMeanValue, finalStdDevRamValue, finalRamModeValue, finalRamMedianValue, finalRamMinValue, finalRamMaxValue);

        Console.WriteLine("- Hard Disk general statistics -");
        double finalHardDiskMeanValue = CommonHelper.CalculateMean(hardDiskValues);
        double finalStdDevHardDiskValue = processingService.CalculateStandardDeviation(hardDiskValues);
        double finalHardDiskMedianValue = CommonHelper.CalculateMedian(hardDiskValues);
        double finalHardDiskMinValue = CommonHelper.CalculateMin(hardDiskValues);
        double finalHardDiskMaxValue = CommonHelper.CalculateMax(hardDiskValues);
        List<double> finalHardDiskModeValue = CommonHelper.CalculateMode(hardDiskValues);

        commonService.DisplayGeneralStatistics(finalHardDiskMeanValue, finalStdDevHardDiskValue, finalHardDiskModeValue, finalHardDiskMedianValue, finalHardDiskMinValue, finalHardDiskMaxValue);

        Console.WriteLine("- Screen Size general statistics -");
        double finalScreenSizeMeanValue = CommonHelper.CalculateMean(screenSizeValues);
        double finalStdDevScreenSizeValue = processingService.CalculateStandardDeviation(screenSizeValues);
        double finalScreenSizeMedianValue = CommonHelper.CalculateMedian(screenSizeValues);
        double finalScreenSizeMinValue = CommonHelper.CalculateMin(screenSizeValues);
        double finalScreenSizeMaxValue = CommonHelper.CalculateMax(screenSizeValues);
        List<double> finalScreenSizeModeValue = CommonHelper.CalculateMode(screenSizeValues);

        commonService.DisplayGeneralStatistics(finalScreenSizeMeanValue, finalStdDevScreenSizeValue, finalScreenSizeModeValue, finalScreenSizeMedianValue, finalScreenSizeMinValue, finalScreenSizeMaxValue);

        Console.WriteLine("- Price general statistics -");
        double finalPriceMeanValue = CommonHelper.CalculateMean(priceValues);
        double finalStdDevPriceValue = processingService.CalculateStandardDeviation(priceValues);
        double finalPriceMedianValue = CommonHelper.CalculateMedian(priceValues);
        double finalPriceMinValue = CommonHelper.CalculateMin(priceValues);
        double finalPriceMaxValue = CommonHelper.CalculateMax(priceValues);
        List<double> finalPriceModeValue = CommonHelper.CalculateMode(priceValues);

        commonService.DisplayGeneralStatistics(finalPriceMeanValue, finalStdDevPriceValue, finalPriceModeValue, finalPriceMedianValue, finalPriceMinValue, finalPriceMaxValue);
    }
}