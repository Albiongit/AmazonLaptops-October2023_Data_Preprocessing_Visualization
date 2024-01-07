using System;
using System.Collections.Generic;
using System.Windows;
using System.Drawing;
using System.Linq;
using Application.Helpers;
using Application.Mappers;
using Application.Services;
using LiveCharts.Defaults;
using LiveCharts;
using ScottPlot;
using SharedData.Models;

namespace Data_Visualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ChartValues<ObservablePoint> LaptopData { get; set; }

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
            double[] hardDiskData = processedDataList.Select(l => l.HardDisk.GetValueOrDefault(0.0)).ToArray();
            double[] screenSizeData = processedDataList.Select(l => l.ScreenSize.GetValueOrDefault(0.0)).ToArray();
            double[] priceData = processedDataList.Select(l => l.Price.GetValueOrDefault(0.0)).ToArray();

            // Tab 1 - ScottPlot Bar Chart - RAM frequency
            var wpfPlotRamBarChart = new WpfPlot();

            // Count the frequency of each RAM value
            var ramFrequency = ramData.GroupBy(x => x)
                .Select(g => new { Ram = g.Key, Frequency = g.Count() })
                .OrderBy(x => x.Ram)
                .ToList();

            // Create a bar plot
            foreach(var value in ramFrequency)
            {
                wpfPlotRamBarChart.Plot.AddBar(value.Ram, value.Frequency);
            }

            // Customize plot labels and title
            wpfPlotRamBarChart.Plot.Title("RAM Frequency");
            wpfPlotRamBarChart.Plot.XLabel("RAM(GB)");
            wpfPlotRamBarChart.Plot.YLabel("Frequency");

            // Render the plot
            wpfPlotRamBarChart.Render();

            // Set the WpfPlot control as the content of the third tab
            tabItemTab1.Content = wpfPlotRamBarChart;

            // Tab 2 - ScottPlot Bar Chart - Hard disk frequency
            var wpfPlotHardDiskBarChart = new WpfPlot();

            // Count the frequency of each RAM value
            var hardDiskFrequency = hardDiskData.GroupBy(x => x)
                .Select(g => new { HardDisk = g.Key, Frequency = g.Count() })
                .OrderBy(x => x.HardDisk)
                .ToList();

            // Create a bar plot
            foreach (var value in hardDiskFrequency)
            {
                wpfPlotHardDiskBarChart.Plot.AddBar(value.HardDisk, value.Frequency);
            }

            // Customize plot labels and title
            wpfPlotHardDiskBarChart.Plot.Title("Hard Disk Frequency");
            wpfPlotHardDiskBarChart.Plot.XLabel("Hard Disk(GB)");
            wpfPlotHardDiskBarChart.Plot.YLabel("Frequency");

            // Render the plot
            wpfPlotHardDiskBarChart.Render();

            // Set the WpfPlot control as the content of the third tab
            tabItemTab2.Content = wpfPlotHardDiskBarChart;

            // Tab 3 - ScottPlot Bar Chart - Screen size frequency
            var wpfPlotScreenSizeBarChart = new WpfPlot();

            // Count the frequency of each RAM value
            var screenSizeFrequency = screenSizeData.GroupBy(x => x)
                .Select(g => new { ScreenSize = g.Key, Frequency = g.Count() })
                .OrderBy(x => x.ScreenSize)
                .ToList();

            // Create a bar plot
            foreach (var value in screenSizeFrequency)
            {
                wpfPlotScreenSizeBarChart.Plot.AddBar(value.ScreenSize, value.Frequency);
            }

            // Customize plot labels and title
            wpfPlotScreenSizeBarChart.Plot.Title("Screen Size Frequency");
            wpfPlotScreenSizeBarChart.Plot.XLabel("Screen Size(Inches)");
            wpfPlotScreenSizeBarChart.Plot.YLabel("Frequency");

            // Render the plot
            wpfPlotScreenSizeBarChart.Render();

            // Set the WpfPlot control as the content of the third tab
            tabItemTab3.Content = wpfPlotScreenSizeBarChart;

            // Tab 4 - ScottPlot Bar Chart - Price frequency
            var wpfPlotPriceBarChart = new WpfPlot();

            // Count the frequency of each RAM value
            var priceFrequency = priceData.GroupBy(x => x)
                .Select(g => new { Price = g.Key, Frequency = g.Count() })
                .OrderBy(x => x.Price)
                .ToList();

            // Create a bar plot
            foreach (var value in priceFrequency)
            {
                wpfPlotPriceBarChart.Plot.AddBar(value.Price, value.Frequency);
            }

            // Customize plot labels and title
            wpfPlotPriceBarChart.Plot.Title("Price Frequency");
            wpfPlotPriceBarChart.Plot.XLabel("Price(USD)");
            wpfPlotPriceBarChart.Plot.YLabel("Frequency");

            // Render the plot
            wpfPlotPriceBarChart.Render();

            // Set the WpfPlot control as the content of the third tab
            tabItemTab4.Content = wpfPlotPriceBarChart;

            // Create a pie plot for the distribution of laptops based on RAM - TAB 5
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
            tabItemTab5.Content = wpfPlotRAMDistribution;

            // Create a scatter plot for screen size vs. price - TAB 6
            LaptopData = new ChartValues<ObservablePoint>();

            // Populate the ScatterSeries with screen size and price data
            foreach (var laptop in processedDataList)
            {
                LaptopData.Add(new ObservablePoint(laptop.ScreenSize.GetValueOrDefault(0.0), laptop.Price.GetValueOrDefault(0.0)));
            }

            DataContext = this;
        }
    }
}
