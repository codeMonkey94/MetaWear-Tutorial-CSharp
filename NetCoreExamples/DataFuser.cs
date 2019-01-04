using MbientLab.MetaWear;
using MbientLab.MetaWear.Core;
using MbientLab.MetaWear.Data;
using MbientLab.MetaWear.NetStandard;
using MbientLab.MetaWear.Sensor;
using System;
using System.Threading.Tasks;

namespace NetCoreExamples {
    class DataFuser {
        static void Main(string[] args) {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args) {
            Console.WriteLine($"Connecting to {args[0]}...");

            var metawear = Application.GetMetaWearBoard(args[0]);
            await metawear.InitializeAsync();

            Console.WriteLine($"Configuring {args[0]}...");

            var acc = metawear.GetModule<IAccelerometer>();
            var gyro = metawear.GetModule<IGyroBmi160>();

            await gyro.AngularVelocity.AddRouteAsync(source => source.Buffer().Name("gyro"));
            await acc.Acceleration.AddRouteAsync(source => source.Fuse("gyro").Stream(_ => {
                var array = _.Value<IData[]>();

                // accelerometer is the source input, index 0
                // gyro name is first input, index 1
                Console.WriteLine($"acc = {array[0].Value<Acceleration>()}, gyro = {array[1].Value<Acceleration>()}");
            }));

            gyro.AngularVelocity.Start();
            acc.Acceleration.Start();

            gyro.Start();
            acc.Start();

            await Task.Delay(15000);

            Console.WriteLine("Resetting device");
            await metawear.GetModule<IDebug>().ResetAsync();
        }
    }
}
