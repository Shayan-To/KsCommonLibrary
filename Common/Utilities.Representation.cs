using System;
using System.Collections.Generic;
using System.Text;

using Reflect = System.Reflection;
using SIO = System.IO;

namespace Ks.Common
{
    partial class Utilities
    {
        public class Representation
        {
            public static readonly IReadOnlyList<(string Prefix, int Multiplier, Interval AcceptInterval)> BinaryPrefixes = new[] { ("", 1024, new Interval(0, true, 1000, false)), ("K", 1024, new Interval(0.9, true, 1000, false)), ("M", 1024, new Interval(0.9, true, 1000, false)), ("G", 1024, new Interval(0.9, true, 1000, false)), ("T", 1024, new Interval(0.9, true, 1000, false)), ("P", 1024, new Interval(0.9, true, 1000, false)), ("E", 1024, new Interval(0.9, true, 1000, false)), ("Z", 1024, new Interval(0.9, true, double.PositiveInfinity, true)) }.AsReadOnly();

            public static readonly IReadOnlyList<(string Prefix, int Multiplier, Interval AcceptInterval)> MetricPrefixes = new[] { ("a", 1000, new Interval(0, true, 1000, false)), ("f", 1000, new Interval(1, true, 1000, false)), ("p", 1000, new Interval(1, true, 1000, false)), ("n", 1000, new Interval(1, true, 1000, false)), ("μ", 1000, new Interval(1, true, 1000, false)), ("m", 1000, new Interval(1, true, 10, false)), ("c", 10, new Interval(1, true, 10, false)), ("d", 10, new Interval(1, true, 10, false)), ("", 10, new Interval(1, true, 10, false)), ("da", 10, new Interval(1, true, 10, false)), ("h", 10, new Interval(1, true, 10, false)), ("k", 10, new Interval(1, true, 1000, false)), ("M", 1000, new Interval(1, true, 1000, false)), ("G", 1000, new Interval(1, true, 1000, false)), ("T", 1000, new Interval(1, true, 1000, false)), ("P", 1000, new Interval(1, true, 1000, false)), ("E", 1000, new Interval(1, true, double.PositiveInfinity, true)) }.AsReadOnly();

            public static (double Value, string Prefix) GetPrefixedRepresentation(double Value, IReadOnlyList<(string Prefix, int Multiplier, Interval AcceptInterval)> Prefixes)
            {
                int I;
                for (I = 0; I < Prefixes.Count; I++)
                {
                    if (string.IsNullOrEmpty(Prefixes[I].Prefix))
                    {
                        break;
                    }
                }

                while (true)
                {
                    var Prefix = Prefixes[I];
                    var C = Prefix.AcceptInterval.Compare(System.Math.Abs(Value));
                    if (C < 0)
                    {
                        I -= 1;
                        Value *= Prefix.Multiplier;
                    }
                    else if (C > 0)
                    {
                        I += 1;
                        Prefix = Prefixes[I];
                        Value /= Prefix.Multiplier;
                    }
                    else
                    {
                        break;
                    }
                }

                return (Value, Prefixes[I].Prefix);
            }

            public static string GetFriendlyTimeSpan(TimeSpan Time, TimeSpan MaxError)
            {
                var Units = new (string Name, TimeSpan UnitValue)[]
                {
                    ("ms", TimeSpan.FromMilliseconds(1)),
                    ("s", TimeSpan.FromSeconds(1)),
                    ("min", TimeSpan.FromMinutes(1)),
                    ("h", TimeSpan.FromHours(1)),
                    ("d", TimeSpan.FromDays(1)),
                };

                // We want to be able to show values using a max error value, so we have to remove the unnecessary units.
                // So we will keep the units that are greater than or equal to MaxError.
                // But these are not enough, as they may not show the value with needed precision if no unit is equal to MaxError.
                // We will use in that case A'shari numbers. See below.

                // We want the units that are greater than or equal to Error.
                // And if the last unit is less that Error, we have no choice but to use it alone. So Func is true for the last unit.
                var Start = Algorithm.BinarySearchIn(X => Units[X].UnitValue >= MaxError, 0, Units.Length - 1);
                // The Units with false should not be used.
                // And from the remaining ones, we will use at most Count of them.
                var Count = 2;

                var Res = new StringBuilder();
                var Ticks = Time.Ticks;
                // We will start from the greatest unit, and pick at most Count of them with non-zero Value.
                // ToDo If ticks is negative, the greatest unit will always be non-zero, even if not necessary. (-1 days 23 hours, instead of -1 hours)
                var I = Units.Length - 1;
                for (; I > Start; I--)
                {
                    var Unit = Units[I];
                    var UnitTicks = Unit.UnitValue.Ticks;
                    var Value = Math.FloorDiv(Ticks, UnitTicks);
                    Ticks -= Value * UnitTicks;

                    if (Value != 0)
                    {
                        if (Count == 1)
                        {
                            break;
                        }

                        if (Res.Length != 0)
                        {
                            Res.Append(" ");
                        }

                        Res.Append(Value).Append(Unit.Name.ToLowerInvariant());
                        Count -= 1;
                    }
                }

                {
                    var Unit = Units[I];
                    var UnitTicks = Unit.UnitValue.Ticks;

                    // Just like the units, we have to check whether (0.01 U) is a good unit or not. (Division will move us upwards in the list.)
                    // We will find units less than or equal to MaxError (the bad ones + equals), and choose the first of them (plus the good ones of course).
                    //
                    // And we cannot optimize it by assuming we can do any number of digits when not at Start.
                    // A unit is something about 20-100 times smaller than the previous one, so maybe we are forced to use only one digit.
                    var Prec = 100;
                    while ((UnitTicks / Prec) <= MaxError.Ticks)
                    {
                        Prec /= 10;
                        if (Prec == 0)
                        {
                            // We will end up here only if the unit equals the error.
                            break;
                        }
                    }

                    Prec = (Prec == 0) ? 1 : (Prec * 10);
                    if (Prec > 100)
                    {
                        Prec = 100;
                    }

                    // We assume (Unit / Prec) to be another unit, but print the result divided by Prec.
                    var Value = System.Math.Round(Ticks / (double) (UnitTicks / Prec));
                    if ((Value != 0) | (Res.Length == 0))
                    {
                        if (Res.Length != 0)
                        {
                            Res.Append(" ");
                        }

                        Res.Append(Value / Prec).Append(Unit.Name.ToLowerInvariant());
                    }
                }

                return Res.ToString();
            }

            private static readonly StringBuilder Builder = new StringBuilder();

            public static string GetTimeStamp(DateTime? Time = default, bool Compact = false)
            {
                DateTime Now;
                if (Time.HasValue)
                {
                    Now = Time.Value;
                }
                else
                {
                    Now = DateTime.Now;
                }

                if (Compact)
                {
                    Builder.Clear()
                        .Append(Now.Year.ToStringInv().PadLeft(4, '0'))
                        .Append(Now.Month.ToStringInv().PadLeft(2, '0'))
                        .Append(Now.Day.ToStringInv().PadLeft(2, '0'))
                        .Append(Now.Hour.ToStringInv().PadLeft(2, '0'))
                        .Append(Now.Minute.ToStringInv().PadLeft(2, '0'))
                        .Append(Now.Second.ToStringInv().PadLeft(2, '0'))
                        .Append(Now.Millisecond.ToStringInv().PadLeft(3, '0'));
                }
                else
                {
                    Builder.Clear()
                        .Append(Now.Year.ToStringInv().PadLeft(4, '0'))
                        .Append('-')
                        .Append(Now.Month.ToStringInv().PadLeft(2, '0'))
                        .Append('-')
                        .Append(Now.Day.ToStringInv().PadLeft(2, '0'))
                        .Append(' ')
                        .Append(Now.Hour.ToStringInv().PadLeft(2, '0'))
                        .Append(':')
                        .Append(Now.Minute.ToStringInv().PadLeft(2, '0'))
                        .Append(':')
                        .Append(Now.Second.ToStringInv().PadLeft(2, '0'))
                        .Append('.')
                        .Append(Now.Millisecond.ToStringInv().PadLeft(3, '0'));
                }

                var Stamp = Builder.ToString();
                return Stamp;
            }

            public static (int Hour, bool IsPm) Hour24To12(int Hour)
            {
                var IsPm = false;

                if (Hour >= 12)
                {
                    Hour -= 12;
                    IsPm = true;
                }
                if (Hour == 0)
                {
                    Hour = 12;
                }

                return (Hour, IsPm);
            }

            public static int Hour12To24(int Hour, bool IsPm)
            {
                if (Hour == 12)
                {
                    Hour = 0;
                }

                if (IsPm)
                {
                    Hour += 12;
                }

                return Hour;
            }

            public static (int Year, int Week, int Day) GetYearWeekDay(DateTime d, DayOfWeek firstDayOfWeek)
            {
                var year = d.Year;
                var day = d.DayOfYear - 1;
                var dow = d.DayOfWeek - firstDayOfWeek;
                dow = (dow + 7) % 7;

                if (day < dow)
                {
                    year -= 1;
                    day += DateTime.IsLeapYear(year) ? 366 : 365;
                }

                var dowOf0 = dow - day;
                dowOf0 = ((dowOf0 % 7) + 14) % 7;
                var week = (day + dowOf0) / 7;
                return (year, week, dow);
            }

            public static (int Year, int Month, int Week, int Day) GetYearMonthWeekDay(DateTime d, DayOfWeek firstDayOfWeek)
            {
                var dow = d.DayOfWeek - firstDayOfWeek;
                dow = (dow + 7) % 7;

                d += TimeSpan.FromDays(-dow + 3);

                var year = d.Year;
                var month = d.Month;
                var week = (d.Day - 1) / 7;

                return (year, month, week, dow);
            }
        }
    }
}
