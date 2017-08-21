using System;
using System.IO;
using System.Windows.Forms;

namespace CP_ColorDiff
{
    public partial class FormSettings : System.Windows.Forms.Form
    {
        public FormSettings()
        {
            InitializeComponent();
            
        }

        private void btnLoadDefHSBValues_Click(object sender, EventArgs e)
        {
            var a = sender as Button;
            var s = a != null && a.Text == "По умолчанию" ? "Default" : "Custom";
            var defHSB = new Serialization().DeSerializeObject<DefHSB>(Path.Combine(Application.StartupPath, s + "HSBparameters"));
            // Yellow
            nmY_MinH.Value = (decimal)defHSB.YellowMinHue;
            nmY_MaxH.Value = (decimal)defHSB.YellowMaxHue;
            nmY_MinS.Value = (decimal)defHSB.YellowMinSaturation * 255;
            nmY_MaxS.Value = (decimal)defHSB.YellowMaxSaturation * 255;
            nmY_MinV.Value = (decimal)defHSB.YellowMinBrightness * 255;
            nmY_MaxV.Value = (decimal)defHSB.YellowMaxBrightness * 255;
            // Orange
            nmO_MinH.Value = (decimal)defHSB.OrangeMinHue;
            nmO_MaxH.Value = (decimal)defHSB.OrangeMaxHue;
            nmO_MinS.Value = (decimal)defHSB.OrangeMinSaturation * 255;
            nmO_MaxS.Value = (decimal)defHSB.OrangeMaxSaturation * 255;
            nmO_MinV.Value = (decimal)defHSB.OrangeMinBrightness * 255;
            nmO_MaxV.Value = (decimal)defHSB.OrangeMaxBrightness * 255;
            // Green
            nmG_MinH.Value = (decimal)defHSB.GreenMinHue;
            nmG_MaxH.Value = (decimal)defHSB.GreenMaxHue;
            nmG_MinS.Value = (decimal)defHSB.GreenMinSaturation * 255;
            nmG_MaxS.Value = (decimal)defHSB.GreenMaxSaturation * 255;
            nmG_MinV.Value = (decimal)defHSB.GreenMinBrightness * 255;
            nmG_MaxV.Value = (decimal)defHSB.GreenMaxBrightness * 255;
            //Pink
            nmP_MinH.Value = (decimal)defHSB.PinkMinHue;
            nmP_MaxH.Value = (decimal)defHSB.PinkMaxHue;
            nmP_MinS.Value = (decimal)defHSB.PinkMinSaturation * 255;
            nmP_MaxS.Value = (decimal)defHSB.PinkMaxSaturation * 255;
            nmP_MinV.Value = (decimal)defHSB.PinkMinBrightness * 255;
            nmP_MaxV.Value = (decimal)defHSB.PinkMaxBrightness * 255;
            // Blue
            nmB_MinH.Value = (decimal)defHSB.BlueMinHue;
            nmB_MaxH.Value = (decimal)defHSB.BlueMaxHue;
            nmB_MinS.Value = (decimal)defHSB.BlueMinSaturation * 255;
            nmB_MaxS.Value = (decimal)defHSB.BlueMaxSaturation * 255;
            nmB_MinV.Value = (decimal)defHSB.BlueMinBrightness * 255;
            nmB_MaxV.Value = (decimal)defHSB.BlueMaxBrightness * 255;
            defHSB = null;

        }

        private void btSaveCustomHSBValues_Click(object sender, EventArgs e)
        {
            var defHSB = new DefHSB()
            {
                MinSaturation =
                    (double)
                    Math.Min(nmY_MinS.Value,
                        Math.Min(nmO_MinS.Value, Math.Min(nmG_MinS.Value, Math.Min(nmP_MinS.Value, nmB_MinS.Value))))/255,
                MinBrightness = (double)Math.Min(nmY_MinV.Value,
                        Math.Min(nmO_MinV.Value, Math.Min(nmG_MinV.Value, Math.Min(nmP_MinV.Value, nmB_MinV.Value))))/255,
                YellowMinHue = (double)nmY_MinH.Value,
                YellowMaxHue = (double)nmY_MaxH.Value,
                YellowMinSaturation = (double)nmY_MinS.Value/255,
                YellowMaxSaturation = (double)nmY_MaxS.Value/255,
                YellowMinBrightness = (double)nmY_MinV.Value/255,
                YellowMaxBrightness = (double)nmY_MaxV.Value/255,
                OrangeMinHue = (double)nmO_MinH.Value,
                OrangeMaxHue = (double)nmO_MaxH.Value,
                OrangeMinSaturation = (double)nmO_MinS.Value / 255,
                OrangeMaxSaturation = (double)nmO_MaxS.Value / 255,
                OrangeMinBrightness = (double)nmO_MinV.Value / 255,
                OrangeMaxBrightness = (double)nmO_MaxV.Value / 255,
                GreenMinHue = (double)nmG_MinH.Value,
                GreenMaxHue = (double)nmG_MaxH.Value,
                GreenMinSaturation = (double)nmG_MinS.Value / 255,
                GreenMaxSaturation = (double)nmG_MaxS.Value / 255,
                GreenMinBrightness = (double)nmG_MinV.Value / 255,
                GreenMaxBrightness = (double)nmG_MaxV.Value / 255,
                PinkMinHue = (double)nmP_MinH.Value,
                PinkMaxHue = (double)nmP_MaxH.Value,
                PinkMinSaturation = (double)nmP_MinS.Value / 255,
                PinkMaxSaturation = (double)nmP_MaxS.Value / 255,
                PinkMinBrightness = (double)nmP_MinV.Value / 255,
                PinkMaxBrightness = (double)nmP_MaxV.Value / 255,
                BlueMinHue = (double)nmB_MinH.Value,
                BlueMaxHue = (double)nmB_MaxH.Value,
                BlueMinSaturation = (double)nmB_MinS.Value / 255,
                BlueMaxSaturation = (double)nmB_MaxS.Value / 255,
                BlueMinBrightness = (double)nmB_MinV.Value / 255,
                BlueMaxBrightness = (double)nmB_MaxV.Value / 255,
            };
            var s = new Serialization();
            s.SerializeObject(defHSB, Path.Combine(Application.StartupPath, "CustomHSBparameters"));
            s = null;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
