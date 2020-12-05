using System.Diagnostics;
using Microsoft.VisualBasic;
using System;

namespace Ks
{
    namespace Common
    {
        public class ConsoleProgressDrawer
        {

            // ToDo Add ability to draw every second or so.
            // ToDo Add instantaneous speed (current is average speed).

            public ConsoleProgressDrawer(bool SpeedSupported = true)
            {
                if (SpeedSupported)
                    this.StopWatch = new Stopwatch();
                this.ShowSpeed = SpeedSupported;
            }

            private void Draw(string TextBefore, string TextAfter)
            {
                if (this.IsSingleLine)
                    Console.Write(ControlChars.Cr);

                Console.Write(TextBefore);

                Console.Write('[');
                if (this.Total == -1)
                    Console.Write("...");
                else
                {
                    var Width = Console.WindowWidth - 1;
                    var ProgWidth = Math.Min(Width - TextBefore.Length - TextAfter.Length - 2, 100);
                    var ProgFull = System.Convert.ToInt32(((double)ProgWidth / this.Total * this.Amount));
                    var ProgEmp = ProgWidth - ProgFull;

                    Console.Write(new string('#', ProgFull));
                    Console.Write(new string(' ', ProgEmp));
                }
                Console.Write(']');

                Console.Write(TextAfter);
            }

            private string GetString(double Value)
            {
                if (double.IsNaN(Value))
                    return string.Format("{0,6} {1,1}", "???", "");

                if (!this.AddMultiplier)
                    return string.Format("{0,6:F2} {1,1}", Value, "");

                var Rep = Utilities.Representation.GetPrefixedRepresentation(Value, Utilities.Representation.BinaryPrefixes);

                return string.Format("{0,6:F2} {1,1}", Rep.Value, Rep.Prefix);
            }

            private void Draw()
            {
                if (!this.IsDrawing)
                    return;

                var T = new System.Text.StringBuilder(this.Text);

                var ShowTotal = this.ShowTotal & (this.Total != -1);

                if (this.ShowAmount | ShowTotal)
                {
                    if (T.Length != 0)
                        T.Append(" ");
                    T.Append("(");
                    if (this.ShowAmount)
                    {
                        T.Append(this.GetString(this.Amount));
                        if (ShowTotal)
                            T.Append(" / ").Append(this.GetString(this.Total));
                    }
                    else
                        T.Append("Total ").Append(this.GetString(this.Total));
                    T.Append(")");
                }

                if ((T.Length != 0) & this.ShowProgressBar)
                    T.Append(" ");

                var Bef = T.ToString();

                T.Clear();

                if (Bef.Length != 0)
                    T.Append(" ");

                if (this.ShowPercentage & (this.Total != -1))
                {
                    var Per = ((double)this.Amount / this.Total) * 100;
                    var Tmp = Per.ToString("F2").PadLeft(5);
                    if (Tmp.Length > 5)
                        Tmp = Tmp.Substring(0, 5);
                    T.Append(Tmp).Append("%");
                }

                if (this.ShowSpeed)
                {
                    if (T.Length != 0)
                        T.Append(" ");
                    T.AppendFormat("at ")
                     .Append(this.GetString(this.Amount / this.StopWatch.Elapsed.TotalSeconds))
                     .Append("/s");
                }

                var Aft = T.ToString();

                this.Draw(Bef, Aft);
            }

            public void ReportProgress(long Amount)
            {
                this.Amount = Amount;
            }

            public void Reset()
            {
                this.Amount = 0;
                this.StopWatch?.Reset();
            }

            public void StartDrawing()
            {
                this.IsDrawing = true;
            }

            public void StopDrawing()
            {
                this.IsDrawing = false;
            }

            private string _Text;

            public string Text
            {
                get
                {
                    return this._Text;
                }
                set
                {
                    this._Text = value;
                    this.Draw();
                }
            }

            private bool _ShowSpeed = true;

            public bool ShowSpeed
            {
                get
                {
                    return this._ShowSpeed;
                }
                set
                {
                    Verify.False(value & this.StopWatch == null, "Speed is not supported.");
                    this._ShowSpeed = value;
                    this.Draw();
                }
            }

            private bool _ShowAmount = true;

            public bool ShowAmount
            {
                get
                {
                    return this._ShowAmount;
                }
                set
                {
                    this._ShowAmount = value;
                    this.Draw();
                }
            }

            private bool _ShowTotal = true;

            public bool ShowTotal
            {
                get
                {
                    return this._ShowTotal;
                }
                set
                {
                    this._ShowTotal = value;
                    this.Draw();
                }
            }

            private bool _ShowPercentage = true;

            public bool ShowPercentage
            {
                get
                {
                    return this._ShowPercentage;
                }
                set
                {
                    this._ShowPercentage = value;
                    this.Draw();
                }
            }

            private bool _ShowProgressBar = true;

            public bool ShowProgressBar
            {
                get
                {
                    return this._ShowProgressBar;
                }
                set
                {
                    value = value & this.IsSingleLine;
                    this._ShowProgressBar = value;
                    this.Draw();
                }
            }

            private bool _AddMultiplier = true;

            public bool AddMultiplier
            {
                get
                {
                    return this._AddMultiplier;
                }
                set
                {
                    this._AddMultiplier = value;
                    this.Draw();
                }
            }

            private long _Amount;

            public long Amount
            {
                get
                {
                    return this._Amount;
                }
                private set
                {
                    this._Amount = value;
                    if (!this.StopWatch.IsRunning)
                        this.StopWatch.Start();
                    this.Draw();
                }
            }

            private long _Total = 100;

            public long Total
            {
                get
                {
                    return this._Total;
                }
                set
                {
                    this._Total = value;
                    this.Draw();
                }
            }

            private bool _IsSingleLine = true;

            public bool IsSingleLine
            {
                get
                {
                    return this._IsSingleLine;
                }
                set
                {
                    this._IsSingleLine = value;
                    if (!value)
                        this.ShowProgressBar = false;
                    else
                        this.Draw();
                }
            }

            private bool _IsDrawing = false;

            public bool IsDrawing
            {
                get
                {
                    return this._IsDrawing;
                }
                set
                {
                    if (this._IsDrawing != value)
                    {
                        this._IsDrawing = value;

                        if (value)
                            this.Draw();
                        else if (this.IsSingleLine)
                            Console.WriteLine();
                    }
                }
            }

            private readonly Stopwatch StopWatch;
        }
    }
}
