using MbientLab.MetaWear.Core;
using MbientLab.MetaWear.Data;
using MbientLab.MetaWear.NetStandard;
using MbientLab.MetaWear.Sensor;
using System;
using System.Threading.Tasks;

namespace NetCoreExamples {
    class LogAccelerometer {
        static async Task Setup(string[] args) {
            Console.WriteLine($"Connecting to {args[0]}...");

            var metawear = Application.GetMetaWearBoard(args[0]);
            await metawear.InitializeAsync();

            Console.WriteLine($"Configuring {args[0]}...");
            var logging = metawear.GetModule<ILogging>();

            var acc = metawear.GetModule<IAccelerometer>();
            await acc.Acceleration.AddRouteAsync(source => source.Log(_ => {
                Console.WriteLine($"{_.FormattedTimestamp} -> {_.Value<Acceleration>()}");
            }));

            logging.Start();
            acc.Acceleration.Start();
            acc.Start();

            Console.WriteLine("Logging data for 15s");
            await Task.Delay(15000);

            acc.Stop();
            acc.Acceleration.Stop();
            logging.Stop();

            Console.WriteLine("Downloading data");
            metawear.GetModule<ISettings>().EditBleConnParams(maxConnInterval: 7.5f);
            await Task.Delay(1500);
            await logging.DownloadAsync();
            
            await metawear.GetModule<IDebug>().ResetAsync();
        }
    }
}
