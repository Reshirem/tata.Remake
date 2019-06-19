using System;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace tata.Remake.TFunction
{
    public class Cat : IMessageProcess
    {
        Random rand_ = new Random();
        public async Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            const string keyword = "/cat";
            if (Global.msgFilter(me.message, false, s => s.TrimStart().StartsWith(keyword)))
            {
                foreach (var ele in me.message.data)
                {
                    if (ele is ElementText)
                    {
                        var rollp = rand_.Next(0, 749);
                        await client.SendMessageAsync(me.messageType, srcid, new Message(new ElementImage($"http://111.231.102.248/static/cat/{rollp}.jpg")));
                    }
                }
            }

            return false;
        }
    }
}