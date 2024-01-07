using SharedData.Models;
using System.Linq.Expressions;

namespace Application.Extensions;

public static class CommonExtensions
{
    public static (double? mostRepeatedRamValue, double? mostRepeatedHardDiskValue)? FindMostRepeatedValueForRamAndHardDisk(this IQueryable<AmazonLaptopProcessedModel> query, Expression<Func<AmazonLaptopProcessedModel, double?>> filterForRam, Expression<Func<AmazonLaptopProcessedModel, double?>> filterForHardDisk)
    {
        var mostRepeatedRamValue = query
            .GroupBy(filterForRam)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .First();

        var mostRepeatedHardDiskValue = query
            .GroupBy(filterForHardDisk)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .First();

        return (mostRepeatedRamValue, mostRepeatedHardDiskValue);
    }
}
