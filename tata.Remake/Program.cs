using System;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Events.CQResponses;
using cqhttp.Cyan.Events.CQResponses.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;
using cqhttp.Cyan.Messages.CQElements.Base;
using MySql.Data.MySqlClient;

namespace tata.Remake
{
    public class Program
    {
        private static void Main(string[] args)
        {
            CQApiClient client = new CQHTTPClient("http://127.0.0.1:5700", "test", 9000);
            //var connString0 = "Server=127.0.0.1;User ID=root;Password=asd123...;Database=tatatemp";
            //var connString1 = "Server=127.0.0.1;User ID=root;Password=asd123...;Database=tata";
            //var conn0 = new MySqlConnection(connString0);
            //var conn1 = new MySqlConnection(connString1);


            Console.WriteLine($"QQ:{client.self_id},{client.self_nick}");

            Global.botid = client.self_id;

            Global.processers.Add(new TFunction.Repeat());
            Global.processers.Add(new TFunction.Dice());
            Global.processers.Add(new TFunction.Ping());
            Global.processers.Add(new TFunction.Cat());

            client.OnEventAsync += OnEventAsync;
            Console.ReadLine();
        }

        private static async Task<CQResponse> OnEventAsync(CQApiClient client, CQEvent eventObj)
        {
            long srcid = -1;
            MessageEvent msgeve = null;

            if (eventObj is GroupMessageEvent)
            {
                var me = eventObj as GroupMessageEvent;
                msgeve = me;
                srcid = me.group_id;
            }
            else if (eventObj is PrivateMessageEvent)
            {
                var me = eventObj as PrivateMessageEvent;
                msgeve = me;
                srcid = me.sender_id;
            }

            if (msgeve != null && Global.enabled_)
                try
                {
                    foreach (var prcs in Global.processers)
                        if (await prcs.ProcessAsync(client, msgeve, srcid))
                            break;
                }
                catch (Exception e)
                {
                    byte[] img = System.IO.File.ReadAllBytes("E:\\static\\1.jpg");
                    await client.SendMessageAsync(msgeve.messageType, srcid,
                        new Message(new ElementImage(img), new ElementText($"\n我崩了: {e.Message}")));
                }

            return new EmptyResponse();
        }
    }
}