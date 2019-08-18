using System;
using System.Threading.Tasks;

namespace ServerlessSleepless.Host.Services.Interfaces
{
    public interface ISelfAwakeService: IDisposable
    {
        object Status();

        Task Awake(object state);

    }
}
