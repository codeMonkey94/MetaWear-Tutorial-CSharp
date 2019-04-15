using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static MbientLab.Warble.Scanner;

namespace NetCoreExamples {
    class Program {
        static void Main(string[] args) {
            MainAsync(args[]);            
        }

        private static async Task MainAsync(string[] args) {
            //SEARCH FOR DEVICE:
            ScanConnect scanner = new ScanConnect();//PROBABLY NOT NEED; USE AN AWAIT AS WELL
            string myDevice = scanner.ScanForMetaWear();//LOOKS FOR DEVICE
            Console.Write("You selected device: " + myDevice + "\n");//DISPLAY THE SELECTED DEVICE
            //CONNECT TO DEVICE:
            var board = await ScanConnect.connect(myDevice, 2);//RET A TASK<METAWEARBOARD>
            //TEST CONNECTIVITY WITH BEACON FLASH OF LED:
             await LedController.setup(board);
            //TEST TEMPERATURE READINGS:
            await StreamTemperature.setup(board);
            //DISCONNECT FROM DEVICE:
            //CLEAR MACROS               
            Console.Read();
        }
    }
}
