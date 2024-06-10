
// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace SeadQueryCore;

// public class Interval
// {
//     public Interval(decimal lower, decimal upper, int segmentCount = 80, decimal width = 0, bool adjust = true, int precision = 4)
//     {

//         Lower = Math.Round(lower <= upper ? lower : upper, precision);
//         Upper = Math.Round(upper >= lower ? upper : lower, precision);

//         Precision = precision;
//         if (adjust || precision == 0)
//         {
//             Lower = Math.Floor(Lower);
//             Upper = Math.Ceiling(Upper);
//         }

//         if (width > 0)
//             SegmentCount = (int)Math.Ceiling((Upper - Lower) / width);
//         else
//             SegmentCount = segmentCount;
//     }

//     public Interval(List<decimal> values, decimal width = 0, bool adjust = true, int precision = 4) : this(values[0], values[1], (int)values.ElementAtOrDefault(2), width, adjust, precision)
//     {
//     }

//     public decimal Lower { get; set; }
//     public decimal Upper { get; set; }
//     public int SegmentCount { get; set; }
//     public int Precision { get; set; }

//     public decimal Width => Math.Round((Upper - Lower) / SegmentCount, Precision);


//     public override string ToString()
//     {
//         return $"[{Lower}, {Upper}, {SegmentCount}]";
//     }

//     public List<decimal> ToList()
//     {
//         return [Lower, Upper, SegmentCount];
//     }

//     public static Interval Create(List<decimal> values, decimal width = 0, bool adjust = true, int precision = 4)
//     {
//         if (values.Count < 2)
//             return null;
//         return new Interval(values, width, adjust, precision);
//     }

// }
