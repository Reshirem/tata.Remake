using System.Threading.Tasks;
using cqhttp.Cyan.ApiCall.Requests;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace tata.Remake.TFunction
{
    public class Debug : IMessageProcess
    {
        public async Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            const string keyword = "/debug";
            if (Global.msgFilter(me.message, false, s => s.TrimStart().StartsWith(keyword)))
            {
                foreach (var ele in me.message.data)
                {
                    if (ele is ElementText)
                    {
                        await client.SendRequestAsync(new SetGroupBanRequest(srcid, me.sender.user_id, 10L));
                    }
                }
            }
            
            return false;
        }
    }
}