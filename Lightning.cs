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

		// Make new array with the same length as the map
		var tiles = new Tile[map.GetLength(0), map.GetLength(1)];

		for (var x = 0; x < map.GetLength(0); x++)
		for (var y = 0; y < map.GetLength(1); y++)
			tiles[x, y] = new Tile(x, y, map[x, y].tileType);
		
		// Open tiles contain tiles for which we will need to calculate lighting
		var openTiles = new List<Tile> { tiles[lightX, lightY] };
		// Closed tiles contain tiles that already calculated
		var closedTiles = new List<Tile>();
		
		var currentIlluminationIntensity = 1f;
		while (currentIlluminationIntensity > 0)
		{
			var tilesCount = openTiles.Count;
			// Calculating lightning for all openTiles
			for (var i = 0; i < tilesCount; i++)
			{
				// Setting illumination for tile
				openTiles[i].illuminationColor = color;
				openTiles[i].illuminationIntensity = currentIlluminationIntensity;
				// For each tile neighbour
				for (var j = 0; j < Neighbours.Count; j++)
				{
					var neighbour = Neighbours[j];
					var position = openTiles[i].position + neighbour;

					// If it is out of map, return
					if (position.x >= tiles.GetLength(0) || position.x < 0 ||
					    position.y >= tiles.GetLength(1) || position.y < 0) continue;

					// If it is not in closed tiles and in one of available directions
					if (openTiles[i].availableDirections.Contains((Direction)j) &&
					    !closedTiles.Contains(tiles[position.x, position.y]))
						openTiles.Add(tiles[position.x, position.y]);
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
		
		// Copy all tiles to the map
		for (var ix = 0; ix < tiles.GetLength(0); ix++)
		for (var iy = 0; iy < tiles.GetLength(1); iy++)
		{
			// Some math to calculate intensity and color
			map[ix, iy].illuminationIntensity += tiles[ix, iy].illuminationIntensity;
			var lerpFactor = tiles[ix, iy].illuminationIntensity / map[ix, iy].illuminationIntensity;
			map[ix, iy].illuminationColor = map[ix, iy].illuminationColor * (1 - lerpFactor) 
			                                + tiles[ix, iy].illuminationColor * lerpFactor;

			map[ix, iy].illuminationIntensity = Math.Min(1, map[ix, iy].illuminationIntensity);
		}
	}
}