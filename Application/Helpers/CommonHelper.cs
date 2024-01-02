namespace Application.Helpers;

public static class CommonHelper
{
    public static double CalculateMean(List<double> values)
    {
        return values.Average();
    }

    public static List<double> CalculateMode(List<double> values)
    {
        var groupedValues = values.GroupBy(v => v);
        int maxFrequency = groupedValues.Max(g => g.Count());
        return groupedValues.Where(g => g.Count() == maxFrequency).Select(g => g.Key).ToList();
    }

    public static double CalculateMedian(List<double> values)
    {
        var sortedValues = values.OrderBy(v => v).ToList();
        int count = sortedValues.Count;

        if (count % 2 == 0)
        {
            // If the count is even, average the middle two values
            int middleIndex1 = count / 2 - 1;
            int middleIndex2 = count / 2;
            return (sortedValues[middleIndex1] + sortedValues[middleIndex2]) / 2.0;
        }
        else
        {
            // If the count is odd, return the middle value
            int middleIndex = count / 2;
            return sortedValues[middleIndex];
        }
    }

    public static double CalculateMax(List<double> values)
    {
        return values.Max();
    }

    public static double CalculateMin(List<double> values)
    {
        return values.Min();
    }
}
