using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;
using tata.Remake.TFunction.TFLib;

namespace tata.Remake.TFunction
{
    public class Minecraft : IMessageProcess
    {
        public async Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            const string keyword = "/mc";
            const string keyword1 = "status";
            if (Global.msgFilter(me.message, false, s => s.TrimStart().StartsWith(keyword)))
            {
                foreach (var ele in me.message.data)
                {
                    if (ele is ElementText && (ele as ElementText).text.ToLower().Contains(keyword1))
                    {
                        try
                        {
                            string ip = null;
                            IPHostEntry iph = Dns.GetHostEntry("bgp.wintopidc.com");
                            foreach (IPAddress ip1 in iph.AddressList)
                            {
                                ip = ip1.ToString();
                                break;
                            }

                            MinecraftServerInfo a;
                            IPAddress c;
                            c = IPAddress.Parse(ip);
                            IPEndPoint b = new IPEndPoint(c, 5035);
                            a = MinecraftServerInfo.GetServerInformation(b);

                            string statusString = "獭のMinecraft测试服" + "\n在线人数：" +
                                                  a.CurrentPlayerCount + "\n最大人数：" +
                                                  a.MaxPlayerCount + "\n服务器Motd：" +
                                                  a.ServerMotd + "\n版本：" +
                                                  a.MinecraftVersion;
                            
                            await client.SendMessageAsync(me.messageType, srcid,
                                new Message(new ElementText(statusString)));
                        }
                        catch (Exception e)
                        {
                            await client.SendMessageAsync(me.messageType, srcid,
                                new Message(new ElementText("服务器貌似没开？\n"), new ElementText(e.ToString())));
                        }
                    }
                }
            }
            
            return false;
        }
    }
}