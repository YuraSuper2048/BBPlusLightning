namespace TileBasedLightning;

public class Tile
{
	public enum Type
	{
		Straight0 = 0,
		Straight90 = 1,
		Corner0 = 2,
		Corner90 = 3,
		Corner180 = 4,
		Corner270 = 5,
		Open = 6,
		Single0 = 7,
		Single90 = 8,
		Single180 = 9,
		Single270 = 10,
		End0 = 11,
		End90 = 12,
		End180 = 13,
		End270 = 14,
		None = 15
	}

	public readonly Vector2Int position;

	public Color illuminationColor;
	public float illuminationIntensity;

	public Type tileType;

	public Tile(Vector2Int position, Type type)
	{
		this.position = position;
		tileType = type;
	}

	public Tile(int x, int y, Type type)
	{
		position = new Vector2Int(x, y);
		tileType = type;
	}

	public Direction[] availableDirections => tileType switch
	{
		Type.Straight0 => new[] { Direction.Left, Direction.Right },
		Type.Straight90 => new[] { Direction.Up, Direction.Down },
		Type.Corner0 => new[] { Direction.Left, Direction.Down },
		Type.Corner90 => new[] { Direction.Down, Direction.Right },
		Type.Corner180 => new[] { Direction.Right, Direction.Up },
		Type.Corner270 => new[] { Direction.Up, Direction.Left },
		Type.Single0 => new[] { Direction.Left, Direction.Down, Direction.Right },
		Type.Single90 => new[] { Direction.Down, Direction.Right, Direction.Up },
		Type.Single180 => new[] { Direction.Right, Direction.Up, Direction.Left },
		Type.Single270 => new[] { Direction.Up, Direction.Left, Direction.Down },
		Type.End0 => new[] { Direction.Down },
		Type.End90 => new[] { Direction.Right },
		Type.End180 => new[] { Direction.Up },
		Type.End270 => new[] { Direction.Left },
		Type.Open => new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left },
		Type.None => Array.Empty<Direction>(),
		_ => throw new ArgumentOutOfRangeException()
	};

	public bool Equals(Tile other)
	{
		return position.Equals(other.position);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != GetType()) return false;
		return Equals((Tile)obj);
	}

	public override int GetHashCode()
	{
		return position.GetHashCode();
	}
}