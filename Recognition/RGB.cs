using System;

/// <summary>
/// RGB structure.
/// </summary>
public struct RGB
{
    /// <summary>
    /// Gets an empty RGB structure;
    /// </summary>
    public static readonly RGB Empty = new RGB();

    private int r;
    private int g;
    private int b;

    public override int GetHashCode()
    {
        return Red.GetHashCode() ^ Green.GetHashCode() ^ Blue.GetHashCode();
    }

    public RGB(int R, int G, int B)
    {
        this.r = (R > 255) ? 255 : ((R < 0) ? 0 : R);
        this.g = (G > 255) ? 255 : ((G < 0) ? 0 : G);
        this.b = (B > 255) ? 255 : ((B < 0) ? 0 : B);
    }

    /// <summary>
    /// Gets or sets red value.
    /// </summary>
    public int Red
    {
        get
        {
            return r;
        }
        set
        {
            r = (value > 255) ? 255 : ((value < 0) ? 0 : value);
        }
    }

    /// <summary>
    /// Gets or sets red value.
    /// </summary>
    public int Green
    {
        get
        {
            return g;
        }
        set
        {
            g = (value > 255) ? 255 : ((value < 0) ? 0 : value);
        }
    }

    /// <summary>
    /// Gets or sets red value.
    /// </summary>
    public int Blue
    {
        get
        {
            return b;
        }
        set
        {
            b = (value > 255) ? 255 : ((value < 0) ? 0 : value);
        }
    }

    public override bool Equals(Object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        return (this == (RGB)obj);
    }
    
    public static bool operator ==(RGB item1, RGB item2)
    {
        return (
            item1.Red == item2.Red
            && item1.Green == item2.Green
            && item1.Blue == item2.Blue
            );
    }

    public static bool operator !=(RGB item1, RGB item2)
    {
        return (
            item1.Red != item2.Red
            || item1.Green != item2.Green
            || item1.Blue != item2.Blue
            );
    }
}