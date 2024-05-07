/*
Copyright (c) Anaconda, Inc., and Bokeh Contributors
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice,
this list of conditions and the following disclaimer.

Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation
and/or other materials provided with the distribution.

Neither the name of Anaconda nor the names of any contributors
may be used to endorse or promote products derived from this software
without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SeadQueryCore;

public readonly struct TickerInfo(decimal dataLow, decimal dataHigh, decimal interval, int precision = 2)
{
    public decimal DataLow { get; } = dataLow;
    public decimal DataHigh { get; } = dataHigh;
    public decimal Interval { get; } = interval;
    public int Precision { get; } = precision;

    public int StartFactor => (int)Math.Floor(DataLow / Interval);
    public int EndFactor => (int)Math.Floor(DataHigh / Interval);

    public decimal TickLow => StartFactor * Interval;
    public decimal TickHigh => EndFactor * Interval;

    public decimal OuterLow => TickLow - (DataLow < TickLow ? Interval : 0);
    public decimal OuterHigh => TickHigh + (DataHigh > TickHigh ? Interval : 0);

    public int IntervalCount => EndFactor - StartFactor + 1;

    public override string ToString() => $"({TickLow}, {TickHigh}, {Interval})";
    public Tuple<decimal, decimal, decimal> ToTuple() => new Tuple<decimal, decimal, decimal>(TickLow, TickHigh, Interval);
}

public class AdaptiveTicker(
    decimal tickBase = 10.0m,
    List<int> mantissas = null,
    decimal minInterval = 0.0m,
    decimal? maxInterval = null,
    int desiredNumTicks = 6,
    int numMinorTicks = 0
    )
{
    /// <summary>
    /// This class is heavely based on the Bokeh AdaptiveTicker class
    /// </summary>
    private decimal Base { get; } = tickBase;
    private List<int> Mantissas { get; } = mantissas ?? [1, 2, 5];
    private decimal MinInterval { get; } = minInterval;
    private decimal MaxInterval { get; } = maxInterval ?? decimal.MaxValue;
    private int DesiredNumTicks { get; } = desiredNumTicks;
    private int NumMinorTicks { get; } = numMinorTicks;

    public (List<decimal>, List<decimal>, TickerInfo) GetTicks(decimal dataLow, decimal dataHigh, int? desiredNTicks = null)
    {
        return GetTicksNoDefaults(dataLow, dataHigh, desiredNTicks ?? DesiredNumTicks);
    }

    public (List<decimal>, List<decimal>, TickerInfo) GetTicksNoDefaults(decimal dataLow, decimal dataHigh, int desiredNTicks)
    {
        TickerInfo ticker = GetInterval(dataLow, dataHigh, desiredNTicks);

        var majorTicks = Enumerable.Range(ticker.StartFactor, ticker.IntervalCount)
                              .Select(factor => factor * ticker.Interval)
                              .Where(tick => dataLow <= tick && tick <= dataHigh)
                              .ToList();

        var minorTicks = GetMinorTicksNoDefaults(dataLow, dataHigh, ticker.Interval, majorTicks);

        return (majorTicks, minorTicks, ticker);
    }

    private List<decimal> GetMinorTicksNoDefaults(decimal dataLow, decimal dataHigh, decimal interval, List<decimal> majorTicks)
    {
        var minorTicks = new List<decimal>();
        if (NumMinorTicks > 0 && majorTicks.Count != 0)
        {
            decimal minorInterval = interval / NumMinorTicks;
            minorTicks = majorTicks.SelectMany(tick => Enumerable
                .Range(0, NumMinorTicks)
                .Select(x => tick + x * minorInterval)
                .Where(mt => dataLow <= mt && mt <= dataHigh))
                .ToList();
        }

        return minorTicks;
    }

    public decimal GetMinInterval() => MinInterval;

    public decimal GetMaxInterval() => MaxInterval;

    public List<decimal> ExtendedMantissas()
    {
        decimal prefixMantissa = Mantissas.Last() / Base;
        decimal suffixMantissa = Mantissas.First() * Base;
        return [prefixMantissa, .. Mantissas.Select(m => (decimal)m), suffixMantissa];
    }

    public decimal BaseFactor() => MinInterval == 0.0m ? 1.0m : MinInterval;

    public TickerInfo GetInterval((decimal, decimal, int) values)
    {
        return GetInterval(values.Item1, values.Item2, values.Item3);
    }

    public TickerInfo GetInterval(decimal dataLow, decimal dataHigh, int desiredNumberOfTicks)
    {
        decimal dataRange = dataHigh - dataLow;
        decimal idealInterval = GetIdealInterval(dataLow, dataHigh, desiredNumberOfTicks);

        int intervalExponent = (int)Math.Floor(Math.Log((double)(idealInterval / BaseFactor()), (double)Base));
        decimal idealMagnitude = (decimal)Math.Pow((double)Base, intervalExponent) * BaseFactor();

        var candidateMantissas = ExtendedMantissas();

        var errors = candidateMantissas.Select(mantissa => Math.Abs(desiredNumberOfTicks - (dataRange / (mantissa * idealMagnitude)))).ToList();
        decimal bestMantissa = candidateMantissas[ArgMin(errors)];
        decimal interval = Clamp(bestMantissa * idealMagnitude);

        return new TickerInfo(dataLow, dataHigh, interval);

    }

    private decimal Clamp(decimal interval)
    {
        return Math.Max(Math.Min(interval, GetMaxInterval()), GetMinInterval());
    }

    public decimal GetIdealInterval(decimal dataLow, decimal dataHigh, int desiredNumberOfTicks)
    {
        return (dataHigh - dataLow) / desiredNumberOfTicks;
    }

    public int ArgMin(List<decimal> values)
    {
        return values.IndexOf(values.Min());
    }
}
