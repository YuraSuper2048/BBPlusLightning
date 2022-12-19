namespace TileBasedLightning;

public struct Color
{
	public float r;
	public float g;
	public float b;
	public float a;

	public Color(float r, float g, float b, float a)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}

	public Color(float r, float g, float b)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		a = 1f;
	}

	public static Color operator +(Color a, Color b)
	{
		return new(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
	}

	public static Color operator -(Color a, Color b)
	{
		return new(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
	}

	public static Color operator *(Color a, Color b)
	{
		return new(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
	}

	public static Color operator *(Color a, float b)
	{
		return new(a.r * b, a.g * b, a.b * b, a.a * b);
	}

	public static Color operator *(float b, Color a)
	{
		return new(a.r * b, a.g * b, a.b * b, a.a * b);
	}

	public static Color operator /(Color a, float b)
	{
		return new(a.r / b, a.g / b, a.b / b, a.a / b);
	}

	public static Color red => new(1f, 0.0f, 0.0f, 1f);

	public static Color green => new(0.0f, 1f, 0.0f, 1f);

	public static Color blue => new(0.0f, 0.0f, 1f, 1f);

	public static Color white => new(1f, 1f, 1f, 1f);

	public static Color black => new(0.0f, 0.0f, 0.0f, 1f);

	public static Color yellow => new(1f, 0.9215686f, 0.01568628f, 1f);

	public static Color cyan => new(0.0f, 1f, 1f, 1f);

	public static Color magenta => new(1f, 0.0f, 1f, 1f);

	public static Color gray => new(0.5f, 0.5f, 0.5f, 1f);

	public static Color grey => new(0.5f, 0.5f, 0.5f, 1f);

	public static Color clear => new(0.0f, 0.0f, 0.0f, 0.0f);
}