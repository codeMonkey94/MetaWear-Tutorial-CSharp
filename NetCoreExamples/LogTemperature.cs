using MbientLab.MetaWear.Core;
using MbientLab.MetaWear.NetStandard;
using MbientLab.MetaWear.Sensor;
using MbientLab.MetaWear.Sensor.Temperature;
using System;
using System.Threading.Tasks;

namespace NetCoreExamples {
    class LogTemperature {
        static void Main(string[] args) {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args) {
            Console.WriteLine($"Connecting to {args[0]}...");

            var metawear = Application.GetMetaWearBoard(args[0]);
            await metawear.InitializeAsync();

            Console.WriteLine($"Configuring {args[0]}...");
            var logging = metawear.GetModule<ILogging>();

            var temperature = metawear.GetModule<ITemperature>();
            var thermistor = temperature.FindSensors(SensorType.PresetThermistor)[0];

            await thermistor.AddRouteAsync(source => source.Log(_ => {
                Console.WriteLine($"{_.FormattedTimestamp} -> {_.Value<float>()}");
            }));
            var timer = await metawear.ScheduleAsync(1000, false, () => thermistor.Read());

            logging.Start();
            timer.Start();

            Console.WriteLine("Logging data for 15s");
            await Task.Delay(15000);

            timer.Stop();
            logging.Stop();

            Console.WriteLine("Downloading data");
            metawear.GetModule<ISettings>().EditBleConnParams(maxConnInterval: 7.5f);
            await Task.Delay(1500);
            await logging.DownloadAsync();

            await metawear.GetModule<IDebug>().ResetAsync();
        }
    }
}
