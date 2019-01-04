using MbientLab.MetaWear;
using MbientLab.MetaWear.Core;
using MbientLab.MetaWear.Data;
using MbientLab.MetaWear.NetStandard;
using MbientLab.MetaWear.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreExamples {
    class StreamAccelerometer {
        static async Task Setup(string[] args) {
            var metawears = new List<IMetaWearBoard>();
            var samples = new Dictionary<IMetaWearBoard, uint>();

            foreach(var _ in args) {
                var m = Application.GetMetaWearBoard(_);
                await m.InitializeAsync();

                Console.WriteLine($"Connected to {_}");
                metawears.Add(m);
                samples.Add(m, 0);
            }

            foreach (var m in metawears) {
                var settings = m.GetModule<ISettings>();
                settings.EditBleConnParams(maxConnInterval: 7.5f);
                await Task.Delay(1500);

                var acc = m.GetModule<IAccelerometer>();
                acc.Configure(odr: 100f, range: 16f);
                await acc.Acceleration.AddRouteAsync(source => source.Stream(_ => {
                    Console.WriteLine($"{m.MacAddress} -> {_.Value<Acceleration>()}");
                    samples[m]++;
                }));
            }

            foreach(var m in metawears) {
                var acc = m.GetModule<IAccelerometer>();
                acc.Acceleration.Start();
                acc.Start();
            }

            await Task.Delay(1000);

            await Task.WhenAll(metawears.Select(_ => {
                var acc = _.GetModule<IAccelerometer>();
                acc.Stop();
                acc.Acceleration.Stop();

                return _.GetModule<IDebug>().DisconnectAsync();
            }));
            
            foreach(var _ in samples) {
                Console.WriteLine($"{_.Key.MacAddress} -> {_.Value}");
            }
        }
    }
}
