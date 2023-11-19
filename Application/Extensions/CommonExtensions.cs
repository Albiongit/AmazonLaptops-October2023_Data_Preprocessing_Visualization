using SharedData.Models;
using System.Linq.Expressions;

namespace Application.Extensions;

public static class CommonExtensions
{
    public static (int? mostRepeatedRamValue, int? mostRepeatedHardDiskValue)? FindMostRepeatedValueForRamAndHardDisk(this IQueryable<AmazonLaptopProcessedModel> query, Expression<Func<AmazonLaptopProcessedModel, int?>> filterForRam, Expression<Func<AmazonLaptopProcessedModel, int?>> filterForHardDisk)
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
