using MbientLab.MetaWear;
using MbientLab.MetaWear.Builder;
using MbientLab.MetaWear.Core;
using MbientLab.MetaWear.Core.DataProcessor;
using MbientLab.MetaWear.Peripheral;
using MbientLab.MetaWear.Peripheral.Led;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreExamples {
    class LedController {
        internal static async Task Setup(IMetaWearBoard metawear) {
            var colors = Enum.GetValues(typeof(Color)).Cast<Color>();
            var led = metawear.GetModule<ILed>();//CREATES A HANDLE INTO LED CLASS
                foreach (var c in colors) {
                        led.Stop(true);
                        led.EditPattern(c, Pattern.Pulse);//CHANGE PATTERN
                        led.Play();
                }

        }

        static async Task RunAsync(string[] args) {
            var metawear = await ScanConnect.Connect(args[0]);

            Console.WriteLine($"Configuring {args[0]}...");
            await Setup(metawear);

            Console.WriteLine("Press [Enter] to reset the board");
            Console.ReadLine();

            Console.WriteLine("Resetting device...");
            await metawear.GetModule<IDebug>().ResetAsync();
        }
    }
}
