using CsvHelper.Configuration;
using SharedData.Models;

namespace Application.Mappers;

public class AmazonLaptopProcessedModelMap : ClassMap<AmazonLaptopProcessedModel>
{
	public AmazonLaptopProcessedModelMap()
	{

        Map(x => x.Brand).Name("Brand");
        Map(x => x.ScreenSize).Name("ScreenSize");
        Map(x => x.HardDisk).Name("HardDisk");
        Map(x => x.Ram).Name("Ram");
        Map(x => x.Price).Name("Price");
    }
}
