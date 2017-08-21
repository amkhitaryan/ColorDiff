using System;

/// <summary>
/// Structure to define HSB.
/// </summary>
public struct HSB
{
    /// <summary>
    /// Gets an empty HSB structure;
    /// </summary>
    public static readonly HSB Empty = new HSB();

    private double _hue;
    private double _saturation;
    private double _brightness;

    public override int GetHashCode()
    {
        return Hue.GetHashCode() ^ Saturation.GetHashCode() ^
            Brightness.GetHashCode();
    }

    /// <summary>
    /// Creates an instance of a HSB structure.
    /// </summary>
    /// <param name="h">Hue value.</param>
    /// <param name="s">Saturation value.</param>
    /// <param name="b">Brightness value.</param>
    public HSB(double h, double s, double b)
    {
        _hue = (h > 360) ? 360 : ((h < 0) ? 0 : h);
        _saturation = (s > 1) ? 1 : ((s < 0) ? 0 : s);
        _brightness = (b > 1) ? 1 : ((b < 0) ? 0 : b);
    }

    /// <summary>
    /// Gets or sets the hue component.
    /// </summary>
    public double Hue
    {
        get
        {
            return _hue;
        }
        set
        {
            if (value > 360) _hue = 360;
            else
            {
                if (value < 0) _hue = 0;
                else _hue = value;
            }
        }
    }

    public override bool Equals(Object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        return (this == (HSB)obj);
    }
    /// <summary>
    /// Gets or sets saturation component.
    /// </summary>
    public double Saturation
    {
        get
        {
            return _saturation;
        }
        set
        {
            _saturation = (value > 1) ? 1 : ((value < 0) ? 0 : value);
        }
    }

    /// <summary>
    /// Gets or sets the brightness component.
    /// </summary>
    public double Brightness
    {
        get
        {
            return _brightness;
        }
        set
        {
            _brightness = (value > 1) ? 1 : ((value < 0) ? 0 : value);
        }
    }

    public static bool operator ==(HSB item1, HSB item2)
    {
        return (
            item1.Hue == item2.Hue
            && item1.Saturation == item2.Saturation
            && item1.Brightness == item2.Brightness
            );
    }

    public static bool operator !=(HSB item1, HSB item2)
    {
        return (
            item1.Hue != item2.Hue
            || item1.Saturation != item2.Saturation
            || item1.Brightness != item2.Brightness
            );
    }
}
