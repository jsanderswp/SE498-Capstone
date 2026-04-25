using StarTrekWeather.Services;
using Xunit;

namespace StarTrekWeatherTests
{
    public class TempServiceTests
    {
        private readonly TempService _service = new();

        [Fact]
        public void GetCurrentTemp_IsWithinRange()
        {
            float min = 18f;
            float max = 67f;

            double result = _service.GetCurrentTemp(max, min);

            Assert.InRange(result, min, max);
        }

        [Fact]
        public void GetCurrentTemp_WhenMinEqualsMax_ReturnsExactValue()
        {
            float min = 25f;
            float max = 25f;

            double result = _service.GetCurrentTemp(max, min);

            Assert.Equal(25.0, result);
        }

        [Fact]
        public void GetCurrentTemp_IsRoundedToOneDecimal()
        {
            float min = 0f;
            float max = 100f;

            double result = _service.GetCurrentTemp(max, min);

            // Check that rounding to 1 decimal doesn't change the value
            Assert.Equal(Math.Round(result, 1), result);
        }

        [Fact]
        public void GetCurrentTemp_WorksWithNegativeRange()
        {
            float min = -80f;
            float max = -15f;

            double result = _service.GetCurrentTemp(max, min);

            Assert.InRange(result, min, max);
        }
    }
}