using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace TileBasedLightning;

/// <summary>
/// Class for lightning calculation
/// </summary>
public static class Lightning
{
	[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
	private static readonly List<Vector2Int> Neighbours = new()
	{
		new(0, 1),
		new(0, -1),
		new(1, 0),
		new(-1, 0),
	};

	/// <summary>
	/// Calculate lightning to the light 
	/// </summary>
	/// <param name="map">Target map</param>
	/// <param name="lightPos">Light position</param>
	/// <param name="color">Light color</param>
	/// <param name="intensity">Light intensity</param>
	/// <param name="falloff">Light falloff (illumination per tile)</param>
	public static void CalculateLightning(ref Tile[,] map, Vector2Int lightPos, Color color, float intensity = 1, float falloff = 0.1f)
	{
		CalculateLightning(ref map, lightPos.x, lightPos.y, color, intensity, falloff);
	}

	/// <summary>
	/// Calculate lightning to the light 
	/// </summary>
	/// <param name="map">Target map</param>
	/// <param name="lightX">Light X</param>
	/// <param name="lightY">Light Y</param>
	/// <param name="color">Light color</param>
	/// <param name="intensity">Light intensity</param>
	/// <param name="falloff">Light falloff (illumination per tile)</param>
	public static void CalculateLightning(ref Tile[,] map, int lightX, int lightY, Color color, float intensity = 1, float falloff = 0.1f)
	{
		if (falloff is < 0 or > 1)
			throw new ArgumentException("Value is not in acceptable range", nameof(falloff));

		// Open tiles contain tiles for which we will need to calculate lighting
		var openTiles = new List<Tile> { map[lightX, lightY] };
		// Closed tiles contain tiles that already calculated
		var closedTiles = new List<Tile>();
		
		var currentIlluminationIntensity = intensity;
		while (currentIlluminationIntensity > 0)
		{
			// Copy all openTiles and loop through copy
			var currentTiles = new List<Tile>(openTiles);
			openTiles.Clear();
			for (var i = 0; i < currentTiles.Count; i++)
			{
				// Math
				var oldIntensity = currentTiles[i].illuminationIntensity;
				currentTiles[i].illuminationIntensity += currentIlluminationIntensity;
				var lerpFactor = oldIntensity / currentTiles[i].illuminationIntensity;
				currentTiles[i].illuminationColor = 
					currentTiles[i].illuminationColor *  lerpFactor
			                                    + color * (1 - lerpFactor);
				
				// For each tile neighbour
				for (var j = 0; j < Neighbours.Count; j++)
				{
					// Get its position
					var position = currentTiles[i].position + Neighbours[j];
					
					// If it is out of map, skip
					if (position.x >= map.GetLength(0) || position.x < 0 ||
					    position.y >= map.GetLength(1) || position.y < 0) continue;
					
					// If it is not in closed tiles and in one of available directions,
					// add it to open tiles
					if((currentTiles[i].mask & (1 << j)) >> j != 1 &&
					   !closedTiles.Contains(map[position.x, position.y]) &&
					   !openTiles.Contains(map[position.x, position.y]))
					{
						openTiles.Add(map[position.x, position.y]);
					}
				}
				
				// Add tile to closed tiles and remove it from open tiles
				closedTiles.Add(currentTiles[i]);
				openTiles.Remove(currentTiles[i]);
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
				Console.Write($"{tiles[x, y].illuminationIntensity:F1}\t");
			}

			Console.WriteLine();
		}

		Console.WriteLine();
		Console.WriteLine();
		Console.WriteLine();
	}
}