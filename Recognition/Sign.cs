using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Класс распознавания жестов
/// </summary>
public class Sign
{
    private List<PointData>[] _fingers;
    private DateTime _timeStamp;

    public Rectangle[] FingerRectangles;

    /// <summary>
    /// Вычисляет номер текущего жеста
    /// </summary>
    /// <param name="fingers">Список пальцев</param>
    /// <returns>Номер текущего жеста</returns>
    public int CurrentSign(List<PointData>[] fingers)
    {
        if (_timeStamp == DateTime.Now)
            return (int)SignID.NOTHING_HAS_DETECTED;

        _timeStamp = DateTime.Now;
        _fingers = fingers;

        for (int i = 0; i < _fingers.Length; i++)
        {
            var item = _fingers[i];
            var thumbRect = new Rectangle((int)item.Min(a => a.X),
                (int)item.Min(a => a.Y),
                (int)item.Max(a => a.X) - (int)item.Min(a => a.X),
                (int)item.Max(a => a.Y) - (int)item.Min(a => a.Y));
            FingerRectangles[i] = thumbRect;
        }

        for (int i = 1; i < _fingers.Length; i++)
        {
            if (Interval(Center(FingerRectangles[(int)FingersID.THUMB]),
                Center(FingerRectangles[i])) <= 50) // 50
                return i;
        }

        return (int)SignID.NOTHING_HAS_DETECTED;
    }

    /// <summary>
    /// Вычисляет центр прямоугольника
    /// </summary>
    /// <param name="rect">Прямоугольник, центр которого ищем</param>
    /// <returns>Центр прямоугольника</returns>
    public Point Center(Rectangle rect)
    {
        return new Point(rect.Left + rect.Width / 2,
            rect.Top + rect.Height / 2);
    }

    /// <summary>
    /// Вычисляет расстояние между двумя точками для прямоугольников
    /// </summary>
    /// <param name="x">Первая точка</param>
    /// <param name="y">Вторая точка</param>
    /// <returns>Расстояние между двумя точками</returns>
    private static int Interval(Point x, Point y)
    {
        return (int)Math.Sqrt((x.X - y.X) * (x.X - y.X) + (x.Y - y.Y) * (x.Y - y.Y));
    }

    public Sign()
    {
        _timeStamp = DateTime.Now;
        FingerRectangles = new Rectangle[5];
    }

}

