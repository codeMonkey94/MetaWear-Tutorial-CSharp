using System.Threading.Tasks;
using MbientLab.MetaWear.Core;
using MbientLab.MetaWear.NetStandard;
using MbientLab.MetaWear.Peripheral;
using MbientLab.MetaWear.Peripheral.Led;

namespace NetCoreExamples {
    class Led {
        static void Main(string[] args) {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args) {
            var metawear = Application.GetMetaWearBoard(args[0]);
            await metawear.InitializeAsync();

            var led = metawear.GetModule<ILed>();
            led.EditPattern(Color.Green, Pattern.Solid);
            led.Play();

            await Task.Delay(5000);
            led.Stop(true);

            await metawear.GetModule<IDebug>().DisconnectAsync();
        }
    }
}