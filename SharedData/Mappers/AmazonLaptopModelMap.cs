using CsvHelper.Configuration;
using SharedData.Models;

namespace SharedData.Mappers;

public class AmazonLaptopModelMap : ClassMap<AmazonLaptopModel>
{
	public AmazonLaptopModelMap()
	{
		Map(x => x.Brand).Name("brand");
		Map(x => x.Model).Name("model");
		Map(x => x.ScreenSize).Name("screen_size");
		Map(x => x.Color).Name("color");
		Map(x => x.HardDisk).Name("harddisk");
		Map(x => x.Cpu).Name("cpu");
		Map(x => x.Ram).Name("ram");
		Map(x => x.OS).Name("OS");
		Map(x => x.SpecialFeatures).Name("special_features");
		Map(x => x.Graphics).Name("graphics");
		Map(x => x.GraphicsCoprocessor).Name("graphics_coprocessor");
		Map(x => x.CpuSpeed).Name("cpu_speed");
		Map(x => x.Rating).Name("rating");
		Map(x => x.Price).Name("price");
	}
}
