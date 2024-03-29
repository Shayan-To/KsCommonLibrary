using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using DColor = System.Drawing.Color;

namespace Ks.Common
{
    public class TestingVisualizer
    {
        public TestingVisualizer()
        {
            this.FormTop = 0;
            this.FormLeft = 0;
            this.FormMargin = 35;
            this.FormWidth = 900;
            this.FormHeight = 600;

            this.XStart = 0;
            this.XLength = 1;
            this.YStart = 0;
            this.YLength = 1;
        }

        public void Initialize(Color BorderRectangleColor)
        {
            // this.Form?.Dispose();
            var Bl = this.Form == null;
            if (Bl)
            {
                this.Form = new Form();
            }

            {
                var withBlock = this.Form;
                withBlock.ClientSize = new Size(this.FormWidth + (this.FormMargin * 2), this.FormHeight + (this.FormMargin * 2));
                withBlock.Location = new Point(this.FormLeft, this.FormTop);
                withBlock.StartPosition = FormStartPosition.Manual;
            }
            if (Bl)
            {
                this.Form.Show();
                this.Graphics = Graphics.FromHwnd(this.Form.Handle);
            }

            this.FIntervals[(int) Orientation.X] = new Interval(this.FormMargin, this.FormWidth);
            this.FIntervals[(int) Orientation.Y] = new Interval(this.FormMargin + this.FormHeight, -this.FormHeight);
            this.Intervals[(int) Orientation.X] = new Interval(this.XStart, this.XLength);
            this.Intervals[(int) Orientation.Y] = new Interval(this.YStart, this.YLength);

            this.BorderRectangleColor = BorderRectangleColor;

            this.Clear();
        }

        public void Clear()
        {
            var FIX = this.FIntervals[(int) Orientation.X];
            var FIY = this.FIntervals[(int) Orientation.Y];

            this.Graphics.Clear(this.Form.BackColor);
            using var Pen = new Pen(this.ConvertColor(this.BorderRectangleColor));
            this.Graphics.DrawLine(Pen, this.CreatePoint(FIX.Start, FIY.Start, Orientation.X), this.CreatePoint(FIX.Start + FIX.Length, FIY.Start, Orientation.X));
            this.Graphics.DrawLine(Pen, this.CreatePoint(FIX.Start, FIY.Start, Orientation.X), this.CreatePoint(FIX.Start, FIY.Start + FIY.Length, Orientation.X));
            this.Graphics.DrawLine(Pen, this.CreatePoint(FIX.Start + FIX.Length, FIY.Start + FIY.Length, Orientation.X), this.CreatePoint(FIX.Start + FIX.Length, FIY.Start, Orientation.X));
            this.Graphics.DrawLine(Pen, this.CreatePoint(FIX.Start + FIX.Length, FIY.Start + FIY.Length, Orientation.X), this.CreatePoint(FIX.Start, FIY.Start + FIY.Length, Orientation.X));
        }

        public void DrawFunction(Color Color, IReadOnlyList<double> Ys, double XStart, double XStep)
        {
            this.DrawFunction(Color, Ys.SelectAsList((Y, I) => (XStart + (I * XStep), Y)));
        }

        public void DrawFunction(Color Color, IReadOnlyList<(double X, double Y)> Points)
        {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            // ToDo Why isn't this ever used?
            var FIX = this.Intervals[(int) Orientation.X];
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            var FPoints = new Point[Points.Count - 1 + 1];
            for (var I = 0; I < FPoints.Length; I++)
            {
                var T = Points[I];
                var P = this.ConvertPoint(T.X, T.Y, Orientation.X);
                FPoints[I] = P;
            }

            using var Pen = new Pen(this.ConvertColor(Color));
            this.Graphics.DrawLines(Pen, FPoints);
        }

        public void DrawFunction(Color Color, Func<double, double> Func)
        {
            var FIX = this.Intervals[(int) Orientation.X];
            var N = 1000;
            var Points = new Point[N + 1];
            for (var I = 0; I <= N; I++)
            {
                var X = FIX.Start + ((FIX.Length / N) * I);
                var P = this.ConvertPoint(X, Func.Invoke(X), Orientation.X);
                Points[I] = P;
            }

            using var Pen = new Pen(this.ConvertColor(Color));
            this.Graphics.DrawLines(Pen, Points);
        }

        public void DrawSlope(Color Color, double Value)
        {
            var IX = this.Intervals[(int) Orientation.X];
            var IY = this.Intervals[(int) Orientation.Y];
            var FIX = this.FIntervals[(int) Orientation.X];
            var FIY = this.FIntervals[(int) Orientation.Y];

            Value /= (IY.Length / FIY.Length) / (IX.Length / FIX.Length);

            var A = Math.Atan(Value);
            var X = FIX.Start;
            var Y = FIY.Start + (FIY.Length / 2);
            var X2 = X + (100 * Math.Cos(A));
            var Y2 = Y + (100 * Math.Sin(A));

            using var Pen = new Pen(this.ConvertColor(Color));
            this.Graphics.DrawLine(Pen, this.CreatePoint(X, Y, Orientation.X), this.CreatePoint(X2, Y2, Orientation.X));
        }

        public void DrawPoint(Color Color, double Value, Orientation Orientation, string Caption = "")
        {
            var FIO = this.FIntervals[1 - (int) Orientation];
            var FV = this.ConvertValue(Value, Orientation);

            var Col = this.ConvertColor(Color);
            using var Pen = new Pen(Col);
            using var Brush = new SolidBrush(Col);
            this.Graphics.DrawLine(Pen, this.CreatePoint(FV, FIO.Start, Orientation), this.CreatePoint(FV, FIO.Start + FIO.Length, Orientation));
            this.Graphics.DrawString(Caption, SystemFonts.DefaultFont, Brush, this.CreatePoint(FV, FIO.Start, Orientation));
        }

        private Point CreatePoint(double X, double Y, Orientation Orientation)
        {
            if (Orientation == Orientation.X)
            {
                return new Point(Convert.ToInt32(X), Convert.ToInt32(Y));
            }
            else
            {
                return new Point(Convert.ToInt32(Y), Convert.ToInt32(X));
            }
        }

        private Size CreateSize(double Width, double Height, Orientation Orientation)
        {
            if (Orientation == Orientation.X)
            {
                return new Size(Convert.ToInt32(Width), Convert.ToInt32(Height));
            }
            else
            {
                return new Size(Convert.ToInt32(Height), Convert.ToInt32(Width));
            }
        }

        private double ConvertValue(double Value, Orientation Orientation)
        {
            var I = this.Intervals[(int) Orientation];
            var FI = this.FIntervals[(int) Orientation];
            return (((Value - I.Start) / I.Length) * FI.Length) + FI.Start;
        }

        private Point ConvertPoint(double X, double Y, Orientation Orientation)
        {
            return this.CreatePoint(this.ConvertValue(X, Orientation), this.ConvertValue(Y, Orientation ^ Orientation.Y), Orientation);
        }

#pragma warning disable IDE0051 // Remove unused private members
        private Size ConvertSize(double Width, double Height, Orientation Orientation)
#pragma warning restore IDE0051 // Remove unused private members
        {
            return this.CreateSize(this.ConvertValue(Width, Orientation), this.ConvertValue(Height, Orientation ^ Orientation.Y), Orientation);
        }

        private DColor ConvertColor(Color Color)
        {
            switch (Color)
            {
                case Color.Transparent:
                    return DColor.Transparent;
                case Color.Black:
                    return DColor.Black;
                case Color.White:
                    return DColor.White;
                case Color.Red:
                    return DColor.Red;
                case Color.Green:
                    return DColor.Green;
                case Color.Blue:
                    return DColor.Blue;
                case Color.Cyan:
                    return DColor.Cyan;
                case Color.Magenta:
                    return DColor.Magenta;
                case Color.Yellow:
                    return DColor.Yellow;
                case Color.Gray:
                    return DColor.Gray;
                case Color.Orange:
                    return DColor.Orange;
            }
            Verify.Fail();
            return default;
        }

        public int FormWidth { get; set; }

        public int FormHeight { get; set; }

        public int FormMargin { get; set; }

        public int FormLeft { get; set; }

        public int FormTop { get; set; }

        public double XStart { get; set; }

        public double XLength { get; set; }

        public double YStart { get; set; }

        public double YLength { get; set; }

        private Color BorderRectangleColor;
        private Graphics Graphics;
        private Form Form;
        private readonly Interval[] FIntervals = new Interval[2];
        private readonly Interval[] Intervals = new Interval[2];

        private struct Interval
        {
            public Interval(double Start, double Length)
            {
                this.Start = Start;
                this.Length = Length;
            }

            public readonly double Start;
            public readonly double Length;
        }

        public enum Orientation : int
        {
            X = 0,
            Y = 1
        }

        public enum Color
        {
            Transparent,
            Black,
            White,
            Red,
            Green,
            Blue,
            Cyan,
            Magenta,
            Yellow,
            Gray,
            Orange
        }
    }
}
