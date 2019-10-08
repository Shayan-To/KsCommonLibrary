using System.Collections.Generic;
using System;
using System.Drawing;
using System.Windows.Forms;
using DColor = System.Drawing.Color;

namespace Ks
{
    namespace Common
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
                    this.Form = new Form();
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

                this.FIntervals[(int)Orientation.X] = new Interval(FormMargin, FormWidth);
                this.FIntervals[(int)Orientation.Y] = new Interval(FormMargin + FormHeight, -FormHeight);
                this.Intervals[(int)Orientation.X] = new Interval(XStart, XLength);
                this.Intervals[(int)Orientation.Y] = new Interval(YStart, YLength);

                this.BorderRectangleColor = BorderRectangleColor;

                this.Clear();
            }

            public void Clear()
            {
                var FIX = this.FIntervals[(int)Orientation.X];
                var FIY = this.FIntervals[(int)Orientation.Y];

                this.Graphics.Clear(this.Form.BackColor);
                using (var Pen = new Pen(ConvertColor(BorderRectangleColor)))
                {
                    this.Graphics.DrawLine(Pen, CreatePoint(FIX.Start, FIY.Start, Orientation.X), CreatePoint(FIX.Start + FIX.Length, FIY.Start, Orientation.X));
                    this.Graphics.DrawLine(Pen, CreatePoint(FIX.Start, FIY.Start, Orientation.X), CreatePoint(FIX.Start, FIY.Start + FIY.Length, Orientation.X));
                    this.Graphics.DrawLine(Pen, CreatePoint(FIX.Start + FIX.Length, FIY.Start + FIY.Length, Orientation.X), CreatePoint(FIX.Start + FIX.Length, FIY.Start, Orientation.X));
                    this.Graphics.DrawLine(Pen, CreatePoint(FIX.Start + FIX.Length, FIY.Start + FIY.Length, Orientation.X), CreatePoint(FIX.Start, FIY.Start + FIY.Length, Orientation.X));
                }
            }

            public void DrawFunction(Color Color, IReadOnlyList<double> Ys, double XStart, double XStep)
            {
                this.DrawFunction(Color, Ys.SelectAsList((Y, I) => (XStart + (I * XStep), Y)));
            }

            public void DrawFunction(Color Color, IReadOnlyList<(double X, double Y)> Points)
            {
                var FIX = this.Intervals[(int)Orientation.X];
                var FPoints = new Point[Points.Count - 1 + 1];
                for (var I = 0; I < FPoints.Length; I++)
                {
                    var T = Points[I];
                    var P = ConvertPoint(T.X, T.Y, Orientation.X);
                    FPoints[I] = P;
                }

                using (var Pen = new Pen(ConvertColor(Color)))
                {
                    this.Graphics.DrawLines(Pen, FPoints);
                }
            }

            public void DrawFunction(Color Color, Func<double, double> Func)
            {
                var FIX = this.Intervals[(int)Orientation.X];
                var N = 1000;
                var Points = new Point[N + 1];
                for (var I = 0; I <= N; I++)
                {
                    var X = FIX.Start + ((FIX.Length / N) * I);
                    var P = ConvertPoint(X, Func.Invoke(X), Orientation.X);
                    Points[I] = P;
                }

                using (var Pen = new Pen(ConvertColor(Color)))
                {
                    this.Graphics.DrawLines(Pen, Points);
                }
            }

            public void DrawSlope(Color Color, double Value)
            {
                var IX = this.Intervals[(int)Orientation.X];
                var IY = this.Intervals[(int)Orientation.Y];
                var FIX = this.FIntervals[(int)Orientation.X];
                var FIY = this.FIntervals[(int)Orientation.Y];

                Value /= (IY.Length / FIY.Length) / (IX.Length / FIX.Length);

                var A = Math.Atan(Value);
                var X = FIX.Start;
                var Y = FIY.Start + (FIY.Length / 2);
                var X2 = X + (100 * Math.Cos(A));
                var Y2 = Y + (100 * Math.Sin(A));

                using (var Pen = new Pen(ConvertColor(Color)))
                {
                    this.Graphics.DrawLine(Pen, CreatePoint(X, Y, Orientation.X), CreatePoint(X2, Y2, Orientation.X));
                }
            }

            public void DrawPoint(Color Color, double Value, Orientation Orientation, string Caption = "")
            {
                var FIO = this.FIntervals[1 - (int)Orientation];
                var FV = ConvertValue(Value, Orientation);

                var Col = ConvertColor(Color);
                using (var Pen = new Pen(ConvertColor(Color)))
                {
                    this.Graphics.DrawLine(Pen, this.CreatePoint(FV, FIO.Start, Orientation), this.CreatePoint(FV, FIO.Start + FIO.Length, Orientation));
                }
                using (var Brush = new SolidBrush(ConvertColor(Color)))
                {
                    this.Graphics.DrawString(Caption, SystemFonts.DefaultFont, Brush, CreatePoint(FV, FIO.Start, Orientation));
                }
            }

            private Point CreatePoint(double X, double Y, Orientation Orientation)
            {
                if (Orientation == Orientation.X)
                    return new Point(Convert.ToInt32(X), Convert.ToInt32(Y));
                else
                    return new Point(Convert.ToInt32(Y), Convert.ToInt32(X));
            }

            private Size CreateSize(double Width, double Height, Orientation Orientation)
            {
                if (Orientation == Orientation.X)
                    return new Size(Convert.ToInt32(Width), Convert.ToInt32(Height));
                else
                    return new Size(Convert.ToInt32(Height), Convert.ToInt32(Width));
            }

            private double ConvertValue(double Value, Orientation Orientation)
            {
                var I = this.Intervals[(int)Orientation];
                var FI = this.FIntervals[(int)Orientation];
                return (((Value - I.Start) / I.Length) * FI.Length) + FI.Start;
            }

            private Point ConvertPoint(double X, double Y, Orientation Orientation)
            {
                return CreatePoint(ConvertValue(X, Orientation), ConvertValue(Y, Orientation ^ Orientation.Y), Orientation);
            }

            private Size ConvertSize(double Width, double Height, Orientation Orientation)
            {
                return CreateSize(ConvertValue(Width, Orientation), ConvertValue(Height, Orientation ^ Orientation.Y), Orientation);
            }

            private DColor ConvertColor(Color Color)
            {
                switch (Color)
                {
                    case Color.Transparent:
                        {
                            return DColor.Transparent;
                        }

                    case Color.Black:
                        {
                            return DColor.Black;
                        }

                    case Color.White:
                        {
                            return DColor.White;
                        }

                    case Color.Red:
                        {
                            return DColor.Red;
                        }

                    case Color.Green:
                        {
                            return DColor.Green;
                        }

                    case Color.Blue:
                        {
                            return DColor.Blue;
                        }

                    case Color.Cyan:
                        {
                            return DColor.Cyan;
                        }

                    case Color.Magenta:
                        {
                            return DColor.Magenta;
                        }

                    case Color.Yellow:
                        {
                            return DColor.Yellow;
                        }

                    case Color.Gray:
                        {
                            return DColor.Gray;
                        }

                    case Color.Orange:
                        {
                            return DColor.Orange;
                        }
                }
                Verify.Fail();
                return default;
            }

            private int _FormWidth;

            public int FormWidth
            {
                get
                {
                    return this._FormWidth;
                }
                set
                {
                    this._FormWidth = value;
                }
            }

            private int _FormHeight;

            public int FormHeight
            {
                get
                {
                    return this._FormHeight;
                }
                set
                {
                    this._FormHeight = value;
                }
            }

            private int _FormMargin;

            public int FormMargin
            {
                get
                {
                    return this._FormMargin;
                }
                set
                {
                    this._FormMargin = value;
                }
            }

            private int _FormLeft;

            public int FormLeft
            {
                get
                {
                    return this._FormLeft;
                }
                set
                {
                    this._FormLeft = value;
                }
            }

            private int _FormTop;

            public int FormTop
            {
                get
                {
                    return this._FormTop;
                }
                set
                {
                    this._FormTop = value;
                }
            }

            private double _XStart;

            public double XStart
            {
                get
                {
                    return this._XStart;
                }
                set
                {
                    this._XStart = value;
                }
            }

            private double _XLength;

            public double XLength
            {
                get
                {
                    return this._XLength;
                }
                set
                {
                    this._XLength = value;
                }
            }

            private double _YStart;

            public double YStart
            {
                get
                {
                    return this._YStart;
                }
                set
                {
                    this._YStart = value;
                }
            }

            private double _YLength;

            public double YLength
            {
                get
                {
                    return this._YLength;
                }
                set
                {
                    this._YLength = value;
                }
            }

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
}
