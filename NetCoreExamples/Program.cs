using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static MbientLab.Warble.Scanner;
#pragma warning disable 1608

namespace NetCoreExamples {
    class Program {
        static void Main(string[] args) {
            MainAsync(args).Wait();             
        }

        private static async Task MainAsync(string[] args) {
           // var type = Type.GetType(args[0]);
          //  await (Task) type.GetMethod("RunAsync", BindingFlags.NonPublic | BindingFlags.Static)
          //      .Invoke(null, new object[] { args.TakeLast(args.Length - 1).ToArray() });

            ScanConnect scanner = new ScanConnect();
            var myDevice = scanner.ScanForMetaWear();//LOOKS FOR DEVICE 
            Console.Write("You selected device: " + myDevice + "\n");
            var connectivity = await ScanConnect.Connect(myDevice, 2);
            await LedController.Setup(connectivity);
            Console.Read();

        }
    }
}