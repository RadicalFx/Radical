using System;
using System.Globalization;
using System.Windows.Media.Animation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Topics.Radical.Windows.Presentation.Converters;

namespace Test.Radical.Windows.Presentation.Converters
{
    [TestClass]
    public class TimeSpanToKeyTimeConverterTests
    {
        [TestMethod]
        [TestCategory("Converters")]
        public void Should_be_able_to_convert_a_timespan_into_a_keytime()
        {
            var timespan = TimeSpan.FromSeconds(123);
            var converter = new TimeSpanToKeyTimeConverter();

            var keytime = (KeyTime)converter.Convert(timespan, typeof(KeyTime), null, CultureInfo.InvariantCulture);

            keytime.TimeSpan.Should().Be.EqualTo(timespan);
            keytime.Type.Should().Be.EqualTo(KeyTimeType.TimeSpan);
        }

        [TestMethod]
        [TestCategory("Converters")]
        public void Should_be_able_to_convert_back_a_keytime_into_a_timespan()
        {
            var keytime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(123));
            var converter = new TimeSpanToKeyTimeConverter();

            var timespan = (TimeSpan)converter.ConvertBack(keytime, typeof(TimeSpan), null, CultureInfo.InvariantCulture);

            timespan.Should().Be.EqualTo(keytime.TimeSpan);
        }
    }
}
