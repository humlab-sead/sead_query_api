
using System;
using System.Collections.Generic;

namespace SeadQueryCore;

public class Interval(decimal lower, decimal upper, int segmentCount = 120)
{
    public Interval(List<decimal> values) : this(values[0], values[1])
    {
        if (values.Count > 2 && values[2] > 0)
            SegmentCount = (int)values[2];
    }

    public decimal Lower { get; set; } = lower <= upper ? lower : upper;
    public decimal Upper { get; set; } = upper >= lower ? upper : lower;
    public int SegmentCount { get; set; } = segmentCount;
    public decimal DecimalWidth => (Upper - Lower) / SegmentCount;
    public int IntegerWidth => Math.Max((int)Math.Floor((Lower - Upper) / SegmentCount), 1);

    public int Count => SegmentCount;
    public int Width => IntegerWidth;

    public override string ToString()
    {
        return $"[{Lower}, {Upper}, {Count}]";
    }

    public List<decimal> ToList()
    {
        return [Lower, Upper, Count];
    }

    public static Interval Create(List<decimal> values)
    {
        if (values.Count < 2)
            return null;
        return new Interval(values);
    }

    public Type GetRangeType()
    {
        if (Lower % 1 == 0 && Upper % 1 == 0 && DecimalWidth % 1 == 0)
            return typeof(int);
        return typeof(decimal);
    }

}
