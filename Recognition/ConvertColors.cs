using System;

abstract class ConvertColors
{
    /// <summary>
    /// Converts CIELab to CIEXYZ.
    /// </summary>
    public static XYZ LabtoXYZ(double l, double a, double b)
    {
        double delta = 6.0 / 29.0;

        double fy = (l + 16) / 116.0;
        double fx = fy + (a / 500.0);
        double fz = fy - (b / 200.0);

        return new XYZ(
            (fx > delta) ? XYZ.D65.X * (fx * fx * fx) : (fx - 16.0 / 116.0) * 3 * (
                delta * delta) * XYZ.D65.X,
            (fy > delta) ? XYZ.D65.Y * (fy * fy * fy) : (fy - 16.0 / 116.0) * 3 * (
                delta * delta) * XYZ.D65.Y,
            (fz > delta) ? XYZ.D65.Z * (fz * fz * fz) : (fz - 16.0 / 116.0) * 3 * (
                delta * delta) * XYZ.D65.Z
        );
    }

    /// <summary>
    /// Converts CIEXYZ to CIELab.
    /// </summary>
    public static CIELab XYZtoLab(double x, double y, double z)
    {
        CIELab lab = CIELab.Empty;

        lab.L = 116.0 * Fxyz(y / XYZ.D65.Y) - 16;
        lab.A = 500.0 * (Fxyz(x / XYZ.D65.X) - Fxyz(y / XYZ.D65.Y));
        lab.B = 200.0 * (Fxyz(y / XYZ.D65.Y) - Fxyz(z / XYZ.D65.Z));

        return lab;
    }

    /// <summary>
    /// Converts CIEXYZ to RGB structure.
    /// </summary>
    public static RGB XYZtoRGB(double x, double y, double z)
    {
        double[] clinear = new double[3];
        clinear[0] = x * 3.2410 - y * 1.5374 - z * 0.4986; // red
        clinear[1] = -x * 0.9692 + y * 1.8760 - z * 0.0416; // green
        clinear[2] = x * 0.0556 - y * 0.2040 + z * 1.0570; // blue

        for (int i = 0; i < 3; i++)
        {
            clinear[i] = (clinear[i] <= 0.0031308) ? 12.92 * clinear[i] : (
                1 + 0.055) * Math.Pow(clinear[i], (1.0 / 2.4)) - 0.055;
        }

        return new RGB(
            Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                clinear[0] * 255.0))),
            Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                clinear[1] * 255.0))),
            Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                clinear[2] * 255.0)))
        );
    }

    /// <summary>
    /// XYZ to L*a*b* transformation function.
    /// </summary>
    private static double Fxyz(double t)
    {
        return ((t > 0.008856) ? Math.Pow(t, (1.0 / 3.0)) : (7.787 * t + 16.0 / 116.0));
    }

    /// <summary>
    /// Converts RGB to CIE XYZ (CIE 1931 color space)
    /// </summary>
    public static XYZ RGBtoXYZ(int red, int green, int blue)
    {
        // normalize red, green, blue values
        double rLinear = (double)red / 255.0;
        double gLinear = (double)green / 255.0;
        double bLinear = (double)blue / 255.0;

        // convert to a sRGB form
        double r = (rLinear > 0.04045) ? Math.Pow((rLinear + 0.055) / (
            1 + 0.055), 2.2) : (rLinear / 12.92);
        double g = (gLinear > 0.04045) ? Math.Pow((gLinear + 0.055) / (
            1 + 0.055), 2.2) : (gLinear / 12.92);
        double b = (bLinear > 0.04045) ? Math.Pow((bLinear + 0.055) / (
            1 + 0.055), 2.2) : (bLinear / 12.92);

        // converts
        return new XYZ(
            (r * 0.4124 + g * 0.3576 + b * 0.1805),
            (r * 0.2126 + g * 0.7152 + b * 0.0722),
            (r * 0.0193 + g * 0.1192 + b * 0.9505)
        );
    }

    /// <summary>
    /// Converts RGB to CIELab.
    /// </summary>
    public static CIELab RGBtoLab(int red, int green, int blue)
    {
        XYZ xyz = RGBtoXYZ(red, green, blue);

        return XYZtoLab(xyz.X, xyz.Y, xyz.Z);
    }

    /// <summary>
    /// Converts HSB to RGB.
    /// </summary>
    public static RGB HSBtoRGB(double h, double s, double v)
    {
        double r = 0;
        double g = 0;
        double b = 0;

        if (s == 0)
        {
            r = g = b = v;
        }
        else
        {
            // the color wheel consists of 6 sectors. Figure out which sector
            // you're in.
            double sectorPos = h / 60.0;
            int sectorNumber = (int)(Math.Floor(sectorPos));
            // get the fractional part of the sector
            double fractionalSector = sectorPos - sectorNumber;

            // calculate values for the three axes of the color.
            double p = v * (1.0 - s);
            double q = v * (1.0 - (s * fractionalSector));
            double t = v * (1.0 - (s * (1 - fractionalSector)));

            // assign the fractional colors to r, g, and b based on the sector
            // the angle is in.
            switch (sectorNumber)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                case 5:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }
        }

        return new RGB(
            Convert.ToInt32(Double.Parse(String.Format("{0:0.00}", r * 255.0))),
            Convert.ToInt32(Double.Parse(String.Format("{0:0.00}", g * 255.0))),
            Convert.ToInt32(Double.Parse(String.Format("{0:0.00}", b * 255.0)))
        );
    }
    /// <summary>
    /// Converts CIELab to RGB.
    /// </summary>
    public static RGB LabtoRGB(double l, double a, double b)
    {
        XYZ xyz = LabtoXYZ(l, a, b);
        return XYZtoRGB(xyz.X, xyz.Y, xyz.Z);
    }

    /// <summary>
    /// Converts RGB to HSB.
    /// </summary>
    public static HSB RGBtoHSB(int red, int green, int blue)
    {
        // normalize red, green and blue values
        double r = ((double)red / 255.0);
        double g = ((double)green / 255.0);
        double b = ((double)blue / 255.0);

        // conversion start
        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));

        double h = 0.0;
        if (max == r && g >= b)
        {
            h = 60 * (g - b) / (max - min);
        }
        else if (max == r && g < b)
        {
            h = 60 * (g - b) / (max - min) + 360;
        }
        else if (max == g)
        {
            h = 60 * (b - r) / (max - min) + 120;
        }
        else if (max == b)
        {
            h = 60 * (r - g) / (max - min) + 240;
        }

        double s = (max == 0) ? 0.0 : (1.0 - (min / max));

        return new HSB(h, s, (double)max);
    }
}
