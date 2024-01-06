using System;
using System.Collections.Generic;
using System.Drawing;
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
        [Obsolete]
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

            // Extract features for visualization
            double[] ramData = processedDataList.Select(l => l.Ram.GetValueOrDefault(0.0)).ToArray();
            double[] priceData = processedDataList.Select(l => l.Price.GetValueOrDefault(0.0)).ToArray();

            // Create a bar plot for the distribution of laptops based on RAM - TAB 1
            var wpfPlotRAMDistribution = new WpfPlot();

            // Define RAM ranges
            List<Tuple<int, int>> ramRanges = new List<Tuple<int, int>>
            {
                Tuple.Create(1, 8),
                Tuple.Create(9, 16),
                Tuple.Create(17, 32),
                Tuple.Create(33, 64)
            };

            // Count laptops in each RAM range
            double[] ramDistribution = new double[4];
            string[] labels = { "1-8GB", "9-16GB", "17-32GB", "33-64GB" };

            // Define custom colors for each slice
            Color[] sliceColors = { Color.Red, Color.Green, Color.Blue, Color.Orange };

            int i = 0;

            foreach(var range in ramRanges)
            {
                ramDistribution[i] = processedDataList.Count(l => l.Ram >= range.Item1 && l.Ram <= range.Item2);

                i++;
            }

            wpfPlotRAMDistribution.Plot.PlotPie(ramDistribution, colors: sliceColors, explodedChart: true, showValues: true, showPercentages: true, showLabels: false, sliceLabels: labels);

            wpfPlotRAMDistribution.Plot.Legend();

            wpfPlotRAMDistribution.Plot.Title("Laptops distribution based on the RAM ranges");
            wpfPlotRAMDistribution.Render();

            // Set the WpfPlot control as the content of the first tab
            tabItemTab1.Content = wpfPlotRAMDistribution;
        }
    }
}
