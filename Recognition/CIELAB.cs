using System;

/// <summary>
/// Structure to define CIE L*a*b*.
/// </summary>
public struct CIELab
{
    /// <summary>
    /// Gets an empty CIELab structure.
    /// </summary>
    public static readonly CIELab Empty = new CIELab();

    private double l;
    private double a;
    private double b;

    public override int GetHashCode()
    {
        return L.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode();
    }

    public CIELab(double l, double a, double b)
    {
        this.l = l;
        this.a = a;
        this.b = b;
    }

    /// <summary>
    /// Gets or sets L component.
    /// </summary>
    public double L
    {
        get
        {
            return this.l;
        }
        set
        {
            this.l = value;
        }
    }

    public override bool Equals(Object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        return (this == (CIELab)obj);
    }

    /// <summary>
    /// Gets or sets a component.
    /// </summary>
    public double A
    {
        get
        {
            return this.a;
        }
        set
        {
            this.a = value;
        }
    }

    /// <summary>
    /// Gets or sets a component.
    /// </summary>
    public double B
    {
        get
        {
            return this.b;
        }
        set
        {
            this.b = value;
        }
    }

    public static bool operator ==(CIELab item1, CIELab item2)
    {
        return (
            item1.L == item2.L
            && item1.A == item2.A
            && item1.B == item2.B
        );
    }

    public static bool operator !=(CIELab item1, CIELab item2)
    {
        return (
            item1.L != item2.L
            || item1.A != item2.A
            || item1.B != item2.B
        );
    }

}