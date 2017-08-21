using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;


public class DefHSB
{
    public double YellowMaxHue { get; set; }
    public double YellowMinHue { get; set; }
    public double YellowMaxSaturation { get; set; }
    public double YellowMinSaturation { get; set; }
    public double YellowMaxBrightness { get; set; }
    public double YellowMinBrightness { get; set; }

    public double OrangeMaxHue { get; set; }
    public double OrangeMinHue { get; set; }
    public double OrangeMaxSaturation { get; set; }
    public double OrangeMinSaturation { get; set; }
    public double OrangeMaxBrightness { get; set; }
    public double OrangeMinBrightness { get; set; }

    public double GreenMaxHue { get; set; }
    public double GreenMinHue { get; set; }
    public double GreenMaxSaturation { get; set; }
    public double GreenMinSaturation { get; set; }
    public double GreenMaxBrightness { get; set; }
    public double GreenMinBrightness { get; set; }

    public double PinkMaxHue { get; set; }
    public double PinkMinHue { get; set; }
    public double PinkMaxSaturation { get; set; }
    public double PinkMinSaturation { get; set; }
    public double PinkMaxBrightness { get; set; }
    public double PinkMinBrightness { get; set; }

    public double BlueMaxHue { get; set; }
    public double BlueMinHue { get; set; }
    public double BlueMaxSaturation { get; set; }
    public double BlueMinSaturation { get; set; }
    public double BlueMaxBrightness { get; set; }
    public double BlueMinBrightness { get; set; }

    public double MinSaturation { get; set; }
    public double MinBrightness { get; set; }



    // Конструктор по умолчанию
    // (требуется для сериализации)
    public DefHSB()
    {

    }
}
