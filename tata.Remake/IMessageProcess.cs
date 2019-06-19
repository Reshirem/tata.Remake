using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;

namespace tata.Remake
{
    public interface IMessageProcess
    {
        Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid);
    }
}