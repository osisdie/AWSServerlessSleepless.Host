using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerlessSleepless.Awaker.Abstraction;
using ServerlessSleepless.Awaker.Common;
using ServerlessSleepless.Awaker.Common.Logging;
using ServerlessSleepless.Host.Models;
using ServerlessSleepless.Host.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ServerlessSleepless.Host.Services
{
    public class SelfAwakeService : ISelfAwakeService
    {
        public SelfAwakeService(IConfiguration configuration, params Assembly[] awakerAssemblies)
        {
            Configuration = configuration;
            Logger = CustomLogFactory.CreateLogger(this.GetType());

            var option = new AwakeWorkerOption();
            configuration.Bind($"{this.GetType().FullName}", option);

            Option = option ?? throw new ArgumentNullException(nameof(Configuration) + $":{this.GetType().FullName}");

            if (Option.Enabled && awakerAssemblies.Count() > 0)
            {
                InitializeIfEnabled(awakerAssemblies);
            }
        }

        void InitializeIfEnabled(Assembly[] awakerAssemblies)
        {
            if (false == awakerAssemblies?.Count() > 0) return;

            foreach (var asm in awakerAssemblies)
            {
                var types = from type in asm.GetTypes()
                            where typeof(IPleaseAwakeMyselfService).IsAssignableFrom(type)
                            select type;

                foreach (var t in types)
                {
                    IPleaseAwakeMyselfService instance = (IPleaseAwakeMyselfService)(Activator.CreateInstance(t, Configuration, Serializer));
                    AwakeServices.Add(instance);
                }
            }

            var delay = Option.DelayStartInSecs * 1000;
            if (delay <= 1) delay = 1000;

            var period = Option.PeriodInSecs * 1000;
            if (period <= 1) period = 1000;

            JobTimer = new Timer(
                new TimerCallback(async (o) => await Awake(o)),
                null,
                delay,
                period);
        }


        public object Status()
        {
            var status = SelfAwakerServiceBase.Echo();
            Logger.LogTrace(JsonConvert.SerializeObject(status));

            return status;
        }

        public async Task Awake(object state)
        {
            if (false == AwakeServices.Count() > 0) return;

            foreach (var service in AwakeServices)
            {
                await service.Awake(state);
            }
        }

        public void Dispose()
        {
            if (null != JobTimer)
            {
                JobTimer.Dispose();
            }
        }

        public Timer JobTimer { get; private set; }
        public AwakeWorkerOption Option { get; private set; }
        public List<IPleaseAwakeMyselfService> AwakeServices { get; private set; } = new List<IPleaseAwakeMyselfService>();
        public ILogger Logger { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public ISerializer Serializer { get; private set; } = new CustomSerializer();
    }
}
