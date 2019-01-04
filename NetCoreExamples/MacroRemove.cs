using MbientLab.MetaWear.Core;
using MbientLab.MetaWear.NetStandard;
using System;
using System.Threading.Tasks;

namespace NetCoreExamples {
    class MacroRemove {
        static void Main(string[] args) {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args) {
            Console.WriteLine($"Connecting to {args[0]}...");
            var metawear = Application.GetMetaWearBoard(args[0]);
            await metawear.InitializeAsync();

            Console.WriteLine("Removing macros");
            metawear.GetModule<IMacro>().EraseAll();

            var debug = metawear.GetModule<IDebug>();
            debug.ResetAfterGc();
            await debug.DisconnectAsync();
        }
    }
}
