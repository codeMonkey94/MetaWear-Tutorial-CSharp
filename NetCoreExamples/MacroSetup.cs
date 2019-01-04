using MbientLab.MetaWear.Builder;
using MbientLab.MetaWear.Core;
using MbientLab.MetaWear.NetStandard;
using MbientLab.MetaWear.Peripheral;
using MbientLab.MetaWear.Peripheral.Led;
using MbientLab.MetaWear.Sensor;
using System;
using System.Threading.Tasks;

namespace NetCoreExamples {
    class MacroSetup {
        static async Task Setup(string[] args) {
            Console.WriteLine($"Connecting to {args[0]}...");
            var metawear = Application.GetMetaWearBoard(args[0]);
            await metawear.InitializeAsync();

            Console.WriteLine($"Configuring {args[0]}...");
            var macro = metawear.GetModule<IMacro>();
            macro.StartRecord();

            var led = metawear.GetModule<ILed>();
            var acc = metawear.GetModule<IAccelerometer>();
            await acc.Acceleration.AddRouteAsync(source =>
                source.Map(Function1.Rss)
                    .LowPass(4).Find(Threshold.Binary, 0.5f).Multicast()
                        .To().Filter(Comparison.Eq, -1f).React(token => {
                            led.EditPattern(Color.Blue, Pattern.Solid);
                            led.Play();
                        }).To().Filter(Comparison.Eq, 1f).React(token =>
                            led.Stop(true)
                        )
            );

            acc.Acceleration.Start();
            acc.Start();

            await macro.EndRecordAsync();

            Console.WriteLine("Resetting device");
            await metawear.GetModule<IDebug>().ResetAsync();
        }
    }
}
