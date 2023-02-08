namespace TileBasedLightning;

public class Tile
{
	public readonly Vector2Int position;

	public Color illuminationColor;
	public float illuminationIntensity;

	// 1 bit: Right
	// 2 bit: Left
	// 4 bit: Down
	// 8 bit: Up
	public int mask;

	public Tile(Vector2Int position, int mask)
	{
		this.position = position;
		this.mask = mask;
	}

	public Tile(int x, int y, int mask)
	{
		position = new Vector2Int(x, y);
		this.mask = mask;
	}

	public bool rightAvailable => (mask & (1 << 1)) >> 1 != 1;
	public bool leftAvailable => (mask & (1 << 2)) >> 2 != 1;
	public bool downAvailable => (mask & (1 << 3)) >> 3 != 1;
	public bool upAvailable => (mask & (1 << 4)) >> 4 != 1;

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