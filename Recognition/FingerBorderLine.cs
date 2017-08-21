using System.Drawing;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Контейнер для границы вокруг пальца
/// </summary>
public struct FingerBorderLine
{
    public FingerBorderLine(Point leftTop,
                           Point rightTop,
                           Point leftBottom,
                           Point rightBottom)
    {
        LeftTop = leftTop;
        RightTop = rightTop;
        LeftBottom = leftBottom;
        RightBottom = rightBottom;
    }

    public Point LeftTop
    {
        get;
        set;
    }

    public Point RightTop
    {
        get;
        set;
    }

    public Point LeftBottom
    {
        get;
        set;
    }

    public Point RightBottom
    {
        get;
        set;
    }

    public Point Center => new Point(((GetRightBottomX() + GetLeftTopX()) / 2),
        ((GetRightBottomY() + GetLeftTopY()) / 2));

    public void Check(Point test)
    {
        // Test Left Top
        if (test.Y <= LeftTop.Y && test.X <= LeftTop.X)
        {
            LeftTop = test;
        }
        // Test RightTop
        else if (test.X >= RightTop.X && test.Y <= RightTop.Y)
        {
            RightTop = test;
        }
        // Test LeftBottom
        else if (test.X <= LeftBottom.X && test.Y >= LeftBottom.Y)
        {
            LeftBottom = test;
        }
        // Test RightBottom
        else if (test.X >= RightBottom.X && test.Y >= RightBottom.Y)
        {
            RightBottom = test;
        }
    }

    public int GetLeftTopX()
    {
        var x = new List<int> {LeftTop.X, RightTop.X, LeftBottom.X, RightBottom.X};
        return x.Min();
    }

    public int GetLeftTopY()
    {
        var y = new List<int> {LeftTop.Y, RightTop.Y, LeftBottom.Y, RightBottom.Y};
        return y.Min();
    }

    public int GetRightBottomX()
    {
        var x = new List<int> {LeftTop.X, RightTop.X, LeftBottom.X, RightBottom.X};
        return x.Max();
    }

    public int GetRightBottomY()
    {
        var y = new List<int> {LeftTop.Y, RightTop.Y, LeftBottom.Y, RightBottom.Y};
        return y.Max();
    }

    public override string ToString()
    {
        return string.Format("FingerRectangle: LeftTop={0}, RightTop={1}, LeftBottom={2}, RightBottom={3}",
            LeftTop, RightTop, LeftBottom, RightBottom);
    }
}

