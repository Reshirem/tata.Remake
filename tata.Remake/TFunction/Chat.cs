using System.Net;
using System.Text;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace tata.Remake.TFunction
{
    public class Chat : IMessageProcess
    {
        public async Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            bool isGroup = me is cqhttp.Cyan.Events.CQEvents.GroupMessageEvent;
            if (Global.msgFilter(me.message, !isGroup, s => true))
            {
                foreach (var ele in me.message.data)
                {
                    if (ele is ElementAt)
                    {
                        WebClient mwc = new WebClient();
                        byte[] chatdlb = mwc.DownloadData($"https://api.sc2h.cn/api/maid.php?data=0&sign={srcid}&msg={me.message}");
                        string chats = Encoding.UTF8.GetString(chatdlb);
                        await client.SendMessageAsync(me.messageType, srcid,
                            new Message(new ElementAt(me.sender.user_id), new ElementText($" {me.message}")));
                    }
                }
            }
            
            return false;
        }
    }
}