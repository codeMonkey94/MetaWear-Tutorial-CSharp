using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static MbientLab.Warble.Scanner;

namespace NetCoreExamples {
    class Program {
        static void Main(string[] args) {
            ScanConnect scanner = new ScanConnect();
            string myDevice = scanner.ScanForMetaWear();//LOOKS FOR DEVICE
            Console.Write("You selected device: " + myDevice);
             
            MainAsync(args);
            Console.Read();
        }

        private static async Task MainAsync(string[] args) {
            var type = Type.GetType(args[0]);
            await (Task) type.GetMethod("RunAsync", BindingFlags.NonPublic | BindingFlags.Static)
                .Invoke(null, new object[] { args.TakeLast(args.Length - 1).ToArray() });
        }
    }
}