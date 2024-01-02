namespace Application.Services;

public class CommonService
{
    public void DisplayGeneralStatistics(double mean, List<double> mode, double median, double min, double max)
    {
        Console.WriteLine($"Mean: {mean}");
        Console.WriteLine($"Mode: {string.Join(", ", mode)}");
        Console.WriteLine($"Median: {median}");
        Console.WriteLine($"Min: {min}");
        Console.WriteLine($"Max: {max}\n");
    }
}
