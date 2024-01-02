using SharedData.Models;

namespace Application.Services;

public static class SMOTEService
{
    static Random random = new Random();

    public static List<AmazonLaptopProcessedModel> ApplySMOTE(List<AmazonLaptopProcessedModel> minorityClass, int k, int numSyntheticSamples)
    {
        List<AmazonLaptopProcessedModel> syntheticSamples = new List<AmazonLaptopProcessedModel>();

        for (int i = 0; i < numSyntheticSamples; i++)
        {
            // Choose a random laptop from the minority class
            AmazonLaptopProcessedModel originalLaptop = minorityClass[random.Next(minorityClass.Count)];

            // Find k nearest neighbors for the chosen laptop
            List<AmazonLaptopProcessedModel> nearestNeighbors = FindKNearestNeighbors(originalLaptop, minorityClass, k);

            // Generate a synthetic laptop by interpolating between the chosen laptop and one of its neighbors
            AmazonLaptopProcessedModel syntheticLaptop = GenerateSyntheticLaptop(originalLaptop, nearestNeighbors);

            syntheticSamples.Add(syntheticLaptop);
        }

        return syntheticSamples;
    }
    private static List<AmazonLaptopProcessedModel> FindKNearestNeighbors(AmazonLaptopProcessedModel laptop, List<AmazonLaptopProcessedModel> laptops, int k)
    {
        // Implement the logic to find k nearest neighbors (e.g., using Euclidean distance)
        // Return a list of k nearest neighbors
        return laptops.OrderBy(l => EuclideanDistance(laptop, l)).Take(k).ToList();
    }

    // Function to calculate Euclidean distance between two laptops
    static double EuclideanDistance(AmazonLaptopProcessedModel laptop1, AmazonLaptopProcessedModel laptop2)
    {
        double sumSquaredDifferences = Math.Pow(laptop1.Ram.GetValueOrDefault(0.0) - laptop2.Ram.GetValueOrDefault(0.0), 2) +
                                       Math.Pow(laptop1.HardDisk.GetValueOrDefault(0.0) - laptop2.HardDisk.GetValueOrDefault(0.0), 2) +
                                       Math.Pow(laptop1.ScreenSize.GetValueOrDefault(0.0) - laptop2.ScreenSize.GetValueOrDefault(0.0), 2) +
                                       Math.Pow(laptop1.Price.GetValueOrDefault(0.0) - laptop2.Price.GetValueOrDefault(0.0), 2);

        return Math.Sqrt(sumSquaredDifferences);
    }

    // Function to generate a synthetic laptop by linear interpolation
    private static AmazonLaptopProcessedModel GenerateSyntheticLaptop(AmazonLaptopProcessedModel originalLaptop, List<AmazonLaptopProcessedModel> nearestNeighbors)
    {
        // Choose one of the k nearest neighbors
        AmazonLaptopProcessedModel neighborLaptop = nearestNeighbors[random.Next(nearestNeighbors.Count)];

        // Choose a random value for the interpolation factor (between 0 and 1)
        double alpha = random.NextDouble();

        // Generate the synthetic laptop by linear interpolation
        AmazonLaptopProcessedModel syntheticLaptop = new AmazonLaptopProcessedModel
        {
            Ram = originalLaptop.Ram + alpha * (neighborLaptop.Ram - originalLaptop.Ram),
            HardDisk = originalLaptop.HardDisk + alpha * (neighborLaptop.HardDisk - originalLaptop.HardDisk),
            ScreenSize = originalLaptop.ScreenSize + alpha * (neighborLaptop.ScreenSize - originalLaptop.ScreenSize),
            Price = originalLaptop.Price + alpha * (neighborLaptop.Price - originalLaptop.Price)
        };

        return syntheticLaptop;
    }
}
