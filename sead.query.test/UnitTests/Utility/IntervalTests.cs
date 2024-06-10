using Xunit;
using SeadQueryCore;
using System;

namespace SQT.Infrastructure
{
    public class IntervalTests
    {
        // [Fact]
        // public void Interval_GetScaleDetails()
        // {
        //     var (min, max, steps) = Interval.GetScaleDetails(0, 100);
        //     Assert.Equal(0, min);
        // }


        [Fact]
        public void AdaptiveTicker_()
        {
            var ticker = new AdaptiveTicker();

            var data = ticker.GetInterval(0.0m, 100.0m, 5);
            Assert.Equal((20.0m, 6), (data.Interval, data.IntervalCount));
            Assert.Equal((0.0m, 100.0m), (data.DataLow, data.DataHigh));
            Assert.Equal((0.0m, 100.0m), (data.TickLow, data.TickHigh));
            Assert.Equal((0, 5), (data.StartFactor, data.EndFactor));

            data = ticker.GetInterval(100.0m, 200.0m, 5);
            Assert.Equal((20.0m, 6), (data.Interval, data.IntervalCount));
            Assert.Equal((100.0m, 200.0m), (data.DataLow, data.DataHigh));
            Assert.Equal((100.0m, 200.0m), (data.TickLow, data.TickHigh));
            Assert.Equal((5, 10), (data.StartFactor, data.EndFactor));

            data = ticker.GetInterval(0.0m, 1.0m, 50);
            Assert.Equal((0.02m, 51), (data.Interval, data.IntervalCount));
            Assert.Equal((0.0m, 1.0m), (data.DataLow, data.DataHigh));
            Assert.Equal((0.0m, 1.0m), (data.TickLow, data.TickHigh));
            Assert.Equal((0, 50), (data.StartFactor, data.EndFactor));

            data = ticker.GetInterval(-0.92896m, 1.91m, 50);
            Assert.Equal((0.05m, 58), (data.Interval, data.IntervalCount));
            Assert.Equal((-0.92896m, 1.91m), (data.DataLow, data.DataHigh));
            Assert.Equal((-0.95m, 1.90m), (data.TickLow, data.TickHigh));
            Assert.Equal((-0.95m, 1.95m), (data.OuterLow, data.OuterHigh));
            Assert.Equal((-19, 38), (data.StartFactor, data.EndFactor));

            // data = ticker.GetInterval(0.0m, 1.0m, 50);
            // Assert.Equal(0.02m, data.Interval);

            // var ticks = ticker.GetTicks(-0.92896m, 1.91m, 50, includeSurroundingTicks: true )["major"];
            // Assert.Equal(-0.95m, ticks[0]);
            // Assert.Equal(1.95m, ticks[^1]);

            // ticks = ticker.GetTicks(-0.92896m, 1.91m, 50, includeSurroundingTicks: false )["major"];
            // Assert.Equal(-0.90m, ticks[0]);
            // Assert.Equal(1.90m, ticks[^1]);
            
        }
    }
}
