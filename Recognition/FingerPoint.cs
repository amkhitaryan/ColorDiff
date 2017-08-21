using System.Drawing;

/// <summary>
/// Контейнер для пальцев
/// </summary>
public struct FingerPoint
{
    public Point Point
    {
        get;
        set;
    }
    public Color Color
    {
        get;
        set;
    }
    public HSB HSBColor
    {
        get;
        set;
    }

    public FingerPoint(int x, int y, Color color, HSB hsbColor)
    {
        Point = new Point(x, y);
        Color = color;
        HSBColor = hsbColor;
    }
}

