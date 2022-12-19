namespace TileBasedLightning;

public struct Vector2Int
{
	public int x;
	public int y;

	public float magnitude => MathF.Sqrt(x * x + y * y);

	public Vector2Int(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public static Vector2Int operator +(Vector2Int first, Vector2Int second)
	{
		return new Vector2Int
		(first.x + second.x,
			first.y + second.y);
	}

	public static Vector2Int operator -(Vector2Int first, Vector2Int second)
	{
		return new Vector2Int
		(first.x - second.x,
			first.y - second.y);
	}

	public static Vector2Int operator /(Vector2Int first, Vector2Int second)
	{
		return new Vector2Int
		(first.x / second.x,
			first.y / second.y);
	}

	public static Vector2Int operator *(Vector2Int first, Vector2Int second)
	{
		return new Vector2Int
		(first.x * second.x,
			first.y * second.y);
	}

	public static float Distance(Vector2Int first, Vector2Int second)
	{
		return (first - second).magnitude;
	}

	public float DistanceTo(Vector2Int second)
	{
		return (this - second).magnitude;
	}

	public bool Equals(Vector2Int other)
	{
		return x == other.x && y == other.y;
	}

	public override bool Equals(object? obj)
	{
		return obj is Vector2Int other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(x, y);
	}
}