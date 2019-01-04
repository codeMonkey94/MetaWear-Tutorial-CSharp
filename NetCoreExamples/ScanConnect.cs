using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MbientLab.Warble;
using MbientLab.MetaWear;
using MbientLab.MetaWear.NetStandard;

namespace NetCoreExamples {
    class ScanConnect {
        static async Task Setup(string[] args) {
            int selection = -1;
            List<ScanResult> devices = new List<ScanResult>();

            while (selection == -1) {
                var seen = new HashSet<string>();
                devices = new List<ScanResult>();
                Scanner.OnResultReceived = item => {
                    if (item.HasServiceUuid(Constants.METAWEAR_GATT_SERVICE.ToString()) && !seen.Contains(item.Mac)) {
                        seen.Add(item.Mac);

                        Console.WriteLine($"[{devices.Count}] = {item.Mac} ({item.Name}) ");
                        devices.Add(item);
                    }
                };
                
                Console.WriteLine("Scanning for devices...");
                Scanner.Start();
                await Task.Delay(10000);
                Scanner.Stop();

                Console.Write("Select your device (-1 to rescan): ");
                selection = int.Parse(Console.ReadLine());
            }

            if (selection >= 0) {
                var metawear = Application.GetMetaWearBoard(devices[selection].Mac);

                Console.WriteLine($"Connecting to {devices[selection].Mac}...");
                await metawear.InitializeAsync();

                Console.WriteLine($"Device information: {await metawear.ReadDeviceInformationAsync()}");
                await Task.Delay(5000);

                await metawear.DisconnectAsync();
                Console.WriteLine("Disconnected");
            }
        }
    }
}
