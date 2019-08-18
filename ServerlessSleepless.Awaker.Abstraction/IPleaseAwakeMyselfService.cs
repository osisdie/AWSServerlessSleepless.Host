using System.Threading.Tasks;

namespace ServerlessSleepless.Awaker.Abstraction
{
    public interface IPleaseAwakeMyselfService
    {
        void InitializeIfEnabled();

        Task Awake(params object[] args);
    }
}
