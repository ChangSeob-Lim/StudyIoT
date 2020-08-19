using Bogus;
using Newtonsoft.Json;
using System;

namespace BogusFakeWeatherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var Rooms = new[] { "DinigRoom", "LivingRoom", "BathRoom", "BedRoom", "GuestRoom" }; // 거실, 방, 주방 등 기기들 ID

            var sensorFaker = new Faker<SensorInfo>()
                                .RuleFor(s => s.Dev_Id, f => f.PickRandom(Rooms))
                                .RuleFor(s => s.Curr_Time, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff")) // f=>f.Date.Past(0)
                                .RuleFor(s => s.Temp, f => float.Parse(f.Random.Float(19.0f, 32f).ToString("0.00")))
                                .RuleFor(s => s.Humid, f => float.Parse(f.Random.Float(40.0f, 70f).ToString("0.0")))
                                .RuleFor(s => s.Press, f => float.Parse(f.Random.Float(800f, 1000f).ToString("0.0")));

            var thisValue = sensorFaker.Generate(100);

            Console.WriteLine(JsonConvert.SerializeObject(thisValue, Formatting.Indented));
        }
    }
}
