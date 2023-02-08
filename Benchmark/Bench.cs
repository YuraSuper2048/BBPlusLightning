using BenchmarkDotNet.Attributes;

namespace TileBasedLightning;

[MemoryDiagnoser(false)]
public class Bench
{
	private Random _random;

	private Tile[,] tiles;

	private Vector2Int mapSize;

	[GlobalSetup]
	public void Setup()
	{
		_random = new Random(572454518);
		
		mapSize = new Vector2Int(100, 100);

		tiles = new Tile[mapSize.x, mapSize.y];

		for (var x = 0; x < mapSize.x; x++)
		for (var y = 0; y < mapSize.y; y++)
			tiles[x, y] = new Tile(x, y, _random.Next(0, 15));
	}

	[Benchmark]
	public void GenLight()
	{
		Lightning.CalculateLightning(ref tiles, _random.Next(0, mapSize.x), 
												_random.Next(0, mapSize.y), Color.white);
	}
}