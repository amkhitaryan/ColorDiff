using System.Drawing;

/// <summary>
/// Тип, который содержит координаты точки(X,Y) и ее цвета
/// </summary>
public class PointData
{
    public double X;
    public double Y;
    public Color Color;
    public HSB HSBColor;

    public PointData(double x, double y, Color color, HSB hsbColor)
    {
        X = x;
        Y = y;
        Color = color;
        HSBColor = hsbColor;
    }
}
