namespace Ks.Common
{
    public static class WpfExtensions
    {
        public static System.Windows.Media.Color WPF(this (byte A, byte R, byte G, byte B) argb)
        {
            return System.Windows.Media.Color.FromArgb(argb.A, argb.R, argb.G, argb.B);
        }

        public static (byte A, byte R, byte G, byte B) Argb(this System.Windows.Media.Color color)
        {
            return (color.A, color.R, color.G, color.B);
        }

        public static System.Windows.Point WPF(this Geometry.Point self)
        {
            return new System.Windows.Point(self.X, self.Y);
        }

        public static System.Windows.Vector WPF(this Geometry.Vector self)
        {
            return new System.Windows.Vector(self.X, self.Y);
        }

        public static System.Windows.Size WPF(this Geometry.Size self)
        {
            return new System.Windows.Size(self.Width, self.Height);
        }

        public static System.Windows.Rect WPF(this Geometry.Rect self)
        {
            return new System.Windows.Rect(self.X, self.Y, self.Width, self.Height);
        }

        public static Geometry.Point Ks(this System.Windows.Point self)
        {
            return new Geometry.Point(self.X, self.Y);
        }

        public static Geometry.Vector Ks(this System.Windows.Vector self)
        {
            return new Geometry.Vector(self.X, self.Y);
        }

        public static Geometry.Size Ks(this System.Windows.Size self)
        {
            return new Geometry.Size(self.Width, self.Height);
        }

        public static Geometry.Rect Ks(this System.Windows.Rect self)
        {
            return new Geometry.Rect(self.X, self.Y, self.Width, self.Height);
        }
    }
}
