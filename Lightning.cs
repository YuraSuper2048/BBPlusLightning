using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace TileBasedLightning;

/// <summary>
/// Class for lightning calculation
/// </summary>
public static class Lightning
{
	[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
	private static readonly List<Vector2Int> Neighbours = new()
	{
		new(-1, 0),
		new(0, 1),
		new(1, 0),
		new(0, -1)
	};

	/// <summary>
	/// Calculate lightning to the light 
	/// </summary>
	/// <param name="map">Target map</param>
	/// <param name="lightPos">Light position</param>
	/// <param name="color">Light color</param>
	/// <param name="falloff">Light falloff (illumination per tile)</param>
	public static void CalculateLightning(ref Tile[,] map, Vector2Int lightPos, Color color, float falloff = 0.1f)
	{
		CalculateLightning(ref map, lightPos.x, lightPos.y, color, falloff);
	}

	/// <summary>
	/// Calculate lightning to the light 
	/// </summary>
	/// <param name="map">Target map</param>
	/// <param name="lightX">Light X</param>
	/// <param name="lightY">Light Y</param>
	/// <param name="color">Light color</param>
	/// <param name="falloff">Light falloff (illumination per tile)</param>
	public static void CalculateLightning(ref Tile[,] map, int lightX, int lightY, Color color, float falloff = 0.1f)
	{
		if (falloff is < 0 or > 1)
			throw new ArgumentException("Value is not in acceptable range", nameof(falloff));

		// Open tiles contain tiles for which we will need to calculate lighting
		var openTiles = new List<Tile> { map[lightX, lightY] };
		// Closed tiles contain tiles that already calculated
		var closedTiles = new List<Tile>();
		
		var currentIlluminationIntensity = 1f;
		while (currentIlluminationIntensity > 0)
		{
			var tilesCount = openTiles.Count;
			// Calculating lightning for all openTiles
			for (var i = 0; i < tilesCount; i++)
			{
				// Maths
				var oldIntensity = openTiles[i].illuminationIntensity;
				openTiles[i].illuminationIntensity += currentIlluminationIntensity;
				var lerpFactor = oldIntensity / openTiles[i].illuminationIntensity;
				openTiles[i].illuminationColor = openTiles[i].illuminationColor * (1 - lerpFactor) 
				                                 + color * lerpFactor;

				openTiles[i].illuminationIntensity = Math.Min(1, openTiles[i].illuminationIntensity);
				
				// For each tile neighbour
				for (var j = 0; j < Neighbours.Count; j++)
				{
					var position = openTiles[i].position + Neighbours[j];

					// If it is out of map, skip
					if (position.x >= map.GetLength(0) || position.x < 0 ||
					    position.y >= map.GetLength(1) || position.y < 0) continue;
					
					// If it is not in closed tiles and in one of available directions,
					// add it to open tiles
					if (openTiles[i].availableDirections.Contains((Direction)j) &&
					    !closedTiles.Contains(map[position.x, position.y]) &&
					    !openTiles.Contains(map[position.x, position.y]))
					{
						openTiles.Add(map[position.x, position.y]);
					}
				}
				
				// Adding tile to closed tiles and removing it from open tiles
				closedTiles.Add(openTiles[i]);
				openTiles.Remove(openTiles[i]);
				tilesCount--;
				i--;
			}

			// Decrease current intensity
			currentIlluminationIntensity -= falloff;
		}
	}
	
	/// <summary>
	/// Debug function: Writes map to the console
	/// </summary>
	private static void Print(Tile[,] tiles)
	{
		for (var x = 0; x < tiles.GetLength(0); x++)
		{
			for (var y = 0; y < tiles.GetLength(1); y++)
			{
				if (tiles[x, y].illuminationIntensity < 0.001f)
				{
					Console.Write("\t");
					continue;
				}

				Console.Write($"{tiles[x, y].illuminationIntensity:F1}\t");
			}

			Console.WriteLine();
		}

		Console.WriteLine();
		Console.WriteLine();
		Console.WriteLine();
	}
}