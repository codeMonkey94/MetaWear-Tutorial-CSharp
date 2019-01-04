using MbientLab.MetaWear.Builder;
using MbientLab.MetaWear.Core;
using MbientLab.MetaWear.Core.DataProcessor;
using MbientLab.MetaWear.NetStandard;
using MbientLab.MetaWear.Sensor;
using System;
using System.Threading.Tasks;

namespace NetCoreExamples {
    class AccThsDetector {
        static async Task Setup(string[] args) {
            Console.WriteLine($"Connecting to {args[0]}...");
            var metawear = Application.GetMetaWearBoard(args[0]);
            await metawear.InitializeAsync();

            Console.WriteLine($"Configuring {args[0]}...");
            var acc = metawear.GetModule<IAccelerometer>();
            acc.Configure(odr: 50f, range: 2f);
            await acc.Acceleration.AddRouteAsync(source =>
                source.Map(Function1.Rss)
                    .LowPass(4).Name("lpf").React(token => 
                        metawear.GetModule<IDataProcessor>().Edit<ILowPassEditor>("lpf").Reset()
                    ).Find(Threshold.Binary, 1f).Log(_ => 
                        Console.WriteLine($"threshold crossed: {_.FormattedTimestamp}, {_.Value<byte>()}")
                    )
            );

            var logging = metawear.GetModule<ILogging>();
            logging.Start();
            acc.Acceleration.Start();
            acc.Start();

            Console.WriteLine("Logging data for 10s");
            await Task.Delay(10000);

            acc.Stop();
            acc.Acceleration.Stop();
            logging.Stop();
            await logging.DownloadAsync();

            Console.WriteLine("Resetting device");
            await metawear.GetModule<IDebug>().ResetAsync();
        }
    }
}
