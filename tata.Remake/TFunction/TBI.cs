using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace tata.Remake.TFunction
{
    public class Se : IMessageProcess
    {
        public async Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            const string keyword = "色";
            const string keyword1 = "下班";
            
            if (Global.msgFilter(me.message, false, s => s.TrimStart().Contains(keyword)))
            {
                foreach (var ele in me.message.data)
                {
                    if (ele is ElementText)
                    {
                        byte[] img = System.IO.File.ReadAllBytes("e:\\static\\1.png");
                        await client.SendMessageAsync(me.messageType, srcid, new Message(new ElementImage(img)));
                    }
                }
            }
            if (Global.msgFilter(me.message, false, s => s.TrimStart().Contains(keyword1)))
            {
                foreach (var ele in me.message.data)
                {
                    if (ele is ElementText)
                    {
                        byte[] img = System.IO.File.ReadAllBytes("e:\\static\\2.jpg");
                        await client.SendMessageAsync(me.messageType, srcid, new Message(new ElementImage(img)));
                    }
                }
            }
            
            return false;
        }
    }
}