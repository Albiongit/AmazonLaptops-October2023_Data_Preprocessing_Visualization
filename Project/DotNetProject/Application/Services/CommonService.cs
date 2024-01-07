namespace Application.Services;

public class CommonService
{
    public void DisplayGeneralStatistics(double mean, double stdDev, List<double> mode, double median, double min, double max)
    {
        Console.WriteLine($"Mean: {mean}");
        Console.WriteLine($"Mode: {string.Join(", ", mode)}");
        Console.WriteLine($"Median: {median}");
        Console.WriteLine($"Standard Deviation: {stdDev}");
        Console.WriteLine($"Min: {min}");
        Console.WriteLine($"Max: {max}\n");
    }

    public void DisplayCorrelationMatrix(double[,] correlationMatrix)
    {
        Console.WriteLine("- Correlation Matrix:");
        Console.WriteLine("      Ram       HDisk   SSize   Price");
        for (int i = 0; i < 4; i++)
        {
            Console.Write(i == 0 ? "Ram   " : (i == 1 ? "HDisk " : (i == 2 ? "SSize " : "Price ")));
            for (int j = 0; j < 4; j++)
            {
                Console.Write(correlationMatrix[i, j].ToString("F4") + "\t");
            }
            Console.WriteLine("\n");
        }
    }
}
