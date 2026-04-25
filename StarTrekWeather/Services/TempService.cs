
namespace StarTrekWeather.Services
{
    public class TempService
    {
        private readonly Random _random = new();
        public double GetCurrentTemp(float maxTemp, float minTemp)
        {
            var currentTemp = _random.NextDouble() * (maxTemp - minTemp) + minTemp;
            return Math.Round(currentTemp, 1);
        }
    }
}    