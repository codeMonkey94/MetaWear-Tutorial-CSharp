using MbientLab.MetaWear.Core;
using MbientLab.MetaWear.Data;
using MbientLab.MetaWear.NetStandard;
using MbientLab.MetaWear.Sensor;
using System;
using System.Threading.Tasks;

namespace NetCoreExamples {
    class DataProcessor {
        static void Main(string[] args) {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args) {
            Console.WriteLine($"Connecting to {args[0]}...");

            var metawear = Application.GetMetaWearBoard(args[0]);
            await metawear.InitializeAsync();

            Console.WriteLine($"Configuring {args[0]}...");
            metawear.GetModule<ISettings>().EditBleConnParams(maxConnInterval: 7.5f);
            await Task.Delay(1500);

            var acc = metawear.GetModule<IAccelerometer>();
            await acc.Acceleration.AddRouteAsync(source => source.LowPass(4).Stream(_ => {
                Console.WriteLine($"{metawear.MacAddress} -> {_.Value<Acceleration>()}");
            }));
            acc.Acceleration.Start();
            acc.Start();

            await Task.Delay(10000);
            await metawear.GetModule<IDebug>().ResetAsync();
        }
    }
}
