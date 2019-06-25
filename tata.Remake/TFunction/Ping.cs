using System;
using System.Threading.Tasks;
using cqhttp.Cyan.ApiCall.Requests;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;
using cqhttp.Cyan.Messages.CQElements.Base;

namespace tata.Remake.TFunction
{
    public class Ping : IMessageProcess
    {
        async public Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            const string keyword = "/ping";
            if (Global.msgFilter(me.message, false, s => s.TrimStart().StartsWith(keyword)))
            {
                foreach (var ele in me.message.data)
                {
                    if (ele is ElementText)
                    {
                        await client.SendMessageAsync(me.messageType, srcid,
                            new Message(new ElementAt(me.sender.user_id), new ElementText(" 啪！")));
                    }
                }
            }

            return false;
        }
    }
}