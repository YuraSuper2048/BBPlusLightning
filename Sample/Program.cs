﻿using BenchmarkDotNet.Running;using TileBasedLightning;var map = new[,]{	{0b1011, 0b1010, 0b1000, 0b1000, 0b1000, 0b1000, 0b1000, 0b1000, 0b1000, 0b1011},	{0b0011, 0b0010, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0011},	{0b0011, 0b0010, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0001},	{0b0011, 0b0010, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0011},	{0b0010, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0011},	{0b0011, 0b0010, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0011},	{0b0011, 0b0010, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0011},	{0b0011, 0b0010, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0011},	{0b0011, 0b0010, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0011},	{0b0111, 0b0110, 0b0100, 0b0100, 0b0100, 0b0100, 0b0100, 0b0100, 0b0100, 0b0111},};var mapSize = new Vector2Int(map.GetLength(0), map.GetLength(1));var tiles = new Tile[mapSize.x, mapSize.y];for (var x = 0; x < mapSize.x; x++)for (var y = 0; y < mapSize.y; y++)	tiles[x, y] = new Tile(x, y, map[x, y]);// for (var i = 0; i < 50; i++)// {// 	var x = _random.Next(0, mapSize.x);// 	var y = _random.Next(0, mapSize.y);// 	while (map[x, y] == 15)// 	{// 		x = _random.Next(0, mapSize.x);// 		y = _random.Next(0, mapSize.y);// 	}// 	Lightning.CalculateLightning(ref tiles, x, y, Color.white);// }Lightning.CalculateLightning(ref tiles, 0, 0, Color.green);Lightning.CalculateLightning(ref tiles, 9, 9, Color.red);// Printing mapfor (var x = 0; x < tiles.GetLength(0); x++){	for (var y = 0; y < tiles.GetLength(1); y++)	{		if (tiles[x, y].illuminationIntensity < 0.001f)		{			Console.Write("\t");			continue;		}		if((x, y) == (0,0))		{			Console.ForegroundColor = ConsoleColor.Green;			Console.Write("*\t");			continue;		}		if((x, y) == (9,9))		{			Console.ForegroundColor = ConsoleColor.Red;			Console.Write("*\t");			continue;		}				if(tiles[x,y].illuminationColor.r > 0.5)			Console.ForegroundColor = ConsoleColor.Red;		if(tiles[x,y].illuminationColor.g > 0.5)			Console.ForegroundColor = ConsoleColor.Green;		if(Math.Abs(tiles[x,y].illuminationColor.r - tiles[x,y].illuminationColor.g) < 0.25f)			Console.ForegroundColor = ConsoleColor.Yellow;		Console.Write($"{tiles[x, y].illuminationIntensity:F1}\t");	}	Console.WriteLine();}Console.WriteLine();Console.WriteLine();Console.WriteLine();Console.WriteLine("Press Y to run Benchmark");if(Console.ReadKey().Key == ConsoleKey.Y)	BenchmarkRunner.Run<Bench>();