using Microsoft.Extensions.Configuration;
using ServerlessSleepless.Awaker.Abstraction;
using ServerlessSleepless.Awaker.Common;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ServerlessSleepless.Awaker.BurstMEM
{
    public class SelfAwakeService : SelfAwakerServiceBase, IPleaseAwakeMyselfService
    {
        public SelfAwakeService(IConfiguration configuration, ISerializer serializer)
            : base(configuration, serializer)
        {

        }

        public override async Task TryAwake(params object[] arg)
        {
            var random = new Random().Next(MinMemSizeMB, MaxMemSizeMB);

            IntPtr hglobal = Marshal.AllocHGlobal(1024 * 1024 * random);
            Marshal.FreeHGlobal(hglobal);

            await Task.Delay(1);
        }

        public override void InitializeIfEnabled()
        {
            MinMemSizeMB = Configuration.GetValue<int>($"{this.GetType().FullName}:MinMemSizeMB", 128);
            MaxMemSizeMB = Configuration.GetValue<int>($"{this.GetType().FullName}:MaxMemSizeMB", 512);
        }

        public int MinMemSizeMB { get; private set; }
        public int MaxMemSizeMB { get; private set; }
    }
}