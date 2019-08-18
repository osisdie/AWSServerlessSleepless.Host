using Microsoft.Extensions.Configuration;
using ServerlessSleepless.Awaker.Abstraction;
using ServerlessSleepless.Awaker.Common;
using System.Threading.Tasks;

namespace ServerlessSleepless.Awaker.BurstCPU
{
    public class SelfAwakeService : SelfAwakerServiceBase, IPleaseAwakeMyselfService
    {
        public SelfAwakeService(IConfiguration configuration, ISerializer serializer)
            : base(configuration, serializer)
        {

        }

        public override async Task TryAwake(params object[] arg)
        {
            int[] list = new int[MaxProcessCount];
            var random = new System.Random();
            for (var i = 0; i < MaxProcessCount; ++i)
            {
                list[i] = random.Next(1, int.MaxValue);
            }

            for (var i = 0; i < MaxProcessCount; ++i)
            {
                await Task.Delay(1);
                Shuffle<int>(list);
            }
        }

        public override void InitializeIfEnabled()
        {
            MaxProcessCount = Configuration.GetValue<int>($"{this.GetType().FullName}:MaxProcessCount", 1000000);

        }

        void Shuffle<T>(T[] list)
        {
            var random = new System.Random();
            for (int n = list.Length; n > 1;)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public int MaxProcessCount { get; private set; }
    }
}
