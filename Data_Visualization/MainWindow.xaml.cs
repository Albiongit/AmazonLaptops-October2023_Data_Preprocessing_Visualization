using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Application.Helpers;
using Application.Mappers;
using Application.Services;
using ScottPlot;
using SharedData.Models;

namespace Data_Visualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Specify the path to dataset CSV file
            string csvFile = CsvServiceHelper.GetCsvFilePath("SharedData", "Data", "amazon_laptops_processed.csv");

            // Read the CSV file and create a list of amazon laptops - take only first 10 records for testing
            List<AmazonLaptopProcessedModel> data = CsvServiceHelper.ReadCsv<AmazonLaptopProcessedModel, AmazonLaptopProcessedModelMap>(csvFile);

            // Include processing data service to use processing methods
            DataProcessingService processingService = new DataProcessingService();

            // Load processed dataset
            List<AmazonLaptopProcessedModel> processedDataList = processingService.GetProcessedData(data);

            // Extract features for visualization - just for testing
            double[] ramData = processedDataList.Select(l => l.Ram.GetValueOrDefault(0.0)).ToArray();
            double[] priceData = processedDataList.Select(l => l.Price.GetValueOrDefault(0.0)).ToArray();

            // Create a scatter plot for visualization
            var wpfPlot = new WpfPlot();

            wpfPlot.Plot.AddScatter(ramData, priceData, markerSize: 5);

            // Customize plot
            wpfPlot.Plot.Title("Laptop Visualization");
            wpfPlot.Plot.XLabel("RAM (GB)");
            wpfPlot.Plot.YLabel("Price (USD)");

            wpfPlot.Refresh();

            // Set the WpfPlot control as the content of the window
            Content = wpfPlot;
        }
    }
}
