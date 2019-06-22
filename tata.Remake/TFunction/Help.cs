using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace tata.Remake.TFunction
{
    public class Help : IMessageProcess
    {
        public async Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            const string keyword = "/help";
            if (Global.msgFilter(me.message, false, s => s.TrimStart().StartsWith(keyword)))
            {
                foreach (var ele in me.message.data)
                {
                    if (ele is ElementText)
                    {
                        await client.SendMessageAsync(me.messageType, srcid, new Message(new ElementText("獭獭Remake Ver.Alpha\n" +
                                                                                                         "命令列表：\n" +
                                                                                                         "/cat  猫图\n" +
                                                                                                         "/dice <骰子个数>d<点数上限>  骰子\n" +
                                                                                                         "/nn & /nuannuan  暖暖\n" +
                                                                                                         "/ping  啪\n" +
                                                                                                         "功能还在继续开发w")));
                    }
                }
            }
            
            return false;
        }
    }
}