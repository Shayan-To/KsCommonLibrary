using System;
using System.Collections.Generic;

using Reflect = System.Reflection;
using SIO = System.IO;

namespace Ks.Common
{
    partial class Utilities
    {
        public static class Color
        {
            public static (double R, double G, double B) HslToRgb(double H, double S, double L)
            {
                throw new NotImplementedException();
            }

            public static (double R, double G, double B) HsbToRgb(double H, double S, double B)
            {
                throw new NotImplementedException();
            }

            public static double GammaExpand(double value)
            {
                if (value <= 0.04045)
                {
                    return value / 12.92;
                }
                return System.Math.Pow((value + 0.055) / 1.055, 2.4);
            }

            public static (double R, double G, double B) GammaExpand((double R, double G, double B) color)
            {
                return (GammaExpand(color.R), GammaExpand(color.G), GammaExpand(color.B));
            }

            public static double GammaCompress(double value)
            {
                if (value <= 0.0031308)
                {
                    return value * 12.92;
                }
                return (System.Math.Pow(value, 1 / 2.4) * 1.055) - 0.055;
            }

            public static (double R, double G, double B) GammaCompress((double R, double G, double B) color)
            {
                return (GammaCompress(color.R), GammaCompress(color.G), GammaCompress(color.B));
            }

            public static double GetLuminance((double R, double G, double B) color)
            {
                color = GammaExpand(color);
                return GammaCompress((0.2126 * color.R) + (0.7152 * color.G) + (0.0722 * color.B));
            }
        }
    }
}
