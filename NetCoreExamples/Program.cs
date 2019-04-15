using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static MbientLab.Warble.Scanner;

namespace NetCoreExamples {
    class Program {
        static void Main(string[] args) {
             MainAsync(args).Wait();            
        }

        private static async Task MainAsync(string[] args) {
            //SEARCH FOR DEVICE:
            ScanConnect scanner = new ScanConnect();//PROBABLY NOT NEED; USE AN AWAIT AS WELL
            string myDevice = scanner.ScanForMetaWear();//LOOKS FOR DEVICE
            Console.Write("You selected device: " + myDevice + "\n");//DISPLAY THE SELECTED DEVICE
           
            //CONNECT TO DEVICE:
            var board = await ScanConnect.Connect(myDevice, 2);//RET A TASK<METAWEARBOARD>
          
            //TEST CONNECTIVITY WITH BEACON FLASH OF LED:
             await LedController.Setup(board);
            
            //TEST TEMPERATURE READINGS:
            await StreamTemperature.Setup(board);                        
            Console.Read();//PAUSE

            //DISCONNECT FROM DEVICE:
            await board.DisconnectAsync();//CLOSE USER INTERFACE SESSION WITH DEVICE
            Console.Read();
        }
    }
}
