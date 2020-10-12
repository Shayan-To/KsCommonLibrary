using System;

namespace Ks.Common.Geometry
{
    public struct Vector
    {
        public Vector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector operator +(Vector l, Vector r)
        {
            return new Vector(l.X + r.X, l.Y + r.Y);
        }

        public static Vector operator -(Vector l, Vector r)
        {
            return new Vector(l.X - r.X, l.Y - r.Y);
        }

        public static Vector operator -(Vector r)
        {
            return new Vector(-r.X, -r.Y);
        }

        public static Vector operator *(double l, Vector r)
        {
            return new Vector(l * r.X, l * r.Y);
        }

        public static Vector operator *(Vector l, double r)
        {
            return new Vector(l.X * r, l.Y * r);
        }

        public static Vector operator /(Vector l, double r)
        {
            return new Vector(l.X / r, l.Y / r);
        }

        public static bool operator ==(Vector l, Vector r)
        {
            return l.X == r.X & l.Y == r.Y;
        }

        public static bool operator !=(Vector l, Vector r)
        {
            return l.X != r.X & l.Y != r.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector vector &&
                   this.X == vector.X &&
                   this.Y == vector.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = (hashCode * -1521134295) + this.X.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Y.GetHashCode();
            return hashCode;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }

    public struct Point
    {
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Point operator +(Point l, Vector r)
        {
            return new Point(l.X + r.X, l.Y + r.Y);
        }

        public static Point operator -(Point l, Vector r)
        {
            return new Point(l.X - r.X, l.Y - r.Y);
        }

        public static Vector operator -(Point l, Point r)
        {
            return new Vector(l.X - r.X, l.Y - r.Y);
        }

        public static bool operator ==(Point l, Point r)
        {
            return l.X == r.X & l.Y == r.Y;
        }

        public static bool operator !=(Point l, Point r)
        {
            return l.X != r.X & l.Y != r.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   this.X == point.X &&
                   this.Y == point.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = (hashCode * -1521134295) + this.X.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Y.GetHashCode();
            return hashCode;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }

    public struct Size
    {
        public Size(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static bool operator ==(Size l, Size r)
        {
            return l.Width == r.Width & l.Height == r.Height;
        }

        public static bool operator !=(Size l, Size r)
        {
            return l.Width != r.Width & l.Height != r.Height;
        }

        public override bool Equals(object obj)
        {
            return obj is Size size &&
                   this.Width == size.Width &&
                   this.Height == size.Height;
        }

        public override int GetHashCode()
        {
            var hashCode = 672978199;
            hashCode = (hashCode * -1521134295) + this.Width.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Height.GetHashCode();
            return hashCode;
        }

        public bool IsEmpty => this == Empty;

        public static readonly Size Empty = new Size(double.NegativeInfinity, double.NegativeInfinity);

        public double Width { get; set; }
        public double Height { get; set; }
    }

    public struct Rect
    {
        public Rect(Point location, Size size)
        {
            this.Location = location;
            this.Size = size;
        }

        public Rect(Point corner1, Point corner2)
        {
            this.Location = new(Math.Min(corner1.X, corner2.X), Math.Min(corner1.Y, corner2.Y));
            this.Size = new(Math.Abs(corner1.X - corner2.X), Math.Abs(corner1.Y - corner2.Y));
        }

        public Rect(Point corner, Vector vector) : this(corner, corner + vector)
        {
        }

        public Rect(double x, double y, double width, double height)
        {
            this.Location = new(x, y);
            this.Size = new(width, height);
        }

        public static Rect operator +(Rect l, Vector r)
        {
            return new Rect(l.Location + r, l.Size);
        }

        public static Rect operator -(Rect l, Vector r)
        {
            return new Rect(l.Location - r, l.Size);
        }

        public static bool operator ==(Rect l, Rect r)
        {
            return l.Location == r.Location & l.Size == r.Size;
        }

        public static bool operator !=(Rect l, Rect r)
        {
            return l.Location != r.Location & l.Size != r.Size;
        }

        public override bool Equals(object obj)
        {
            return obj is Rect rect &&
                   this.Location == rect.Location &&
                   this.Size == rect.Size;
        }

        public override int GetHashCode()
        {
            var hashCode = -1153359444;
            hashCode = (hashCode * -1521134295) + this.Location.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Size.GetHashCode();
            return hashCode;
        }

        public static readonly Rect Empty = new Rect(new Point(double.PositiveInfinity, double.PositiveInfinity), Size.Empty);

        public bool IsEmpty => this == Empty;

        public double Width => this.Size.Width;
        public double Height => this.Size.Height;
        public double X => this.Location.X;
        public double Y => this.Location.Y;

        public Point Location { get; set; }
        public Size Size { get; set; }
    }
}
