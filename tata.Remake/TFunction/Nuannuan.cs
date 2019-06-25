using System.Net;
using System.Text;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;
using cqhttp.Cyan.Messages.CQElements.Base;
using Newtonsoft.Json;

namespace tata.Remake.TFunction
{
    public class Nuannuan : IMessageProcess
    {
        public async Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            const string keyword = "/nuannuan";
            const string keyword1 = "/nn";
            const string keyword2 = "text";
            if (Global.msgFilter(me.message, false, s => s.TrimStart().StartsWith(keyword))
                || Global.msgFilter(me.message, false, s => s.TrimStart().StartsWith(keyword1)))
            {
                foreach (var ele in me.message.data)
                {
                    if (ele is ElementText && !(ele as ElementText).text.ToLower().Contains(keyword2))
                    {
                        WebClient mwc = new WebClient();
                        byte[] nndata = mwc.DownloadData("http://nuannuan.yorushika.co:5000/");
                        string nnjson = Encoding.UTF8.GetString(nndata);
                        nnjd nnjsonb = JsonConvert.DeserializeObject<nnjd>(nnjson);

                        if (nnjsonb.content.StartsWith("[CQ:"))
                        {
                            string nnI1 = nnjsonb.content.Replace("[CQ:","");
                            string nnI = nnI1.Replace("]", "");
                            await client.SendMessageAsync(me.messageType, srcid,
                                new Message(new Element(nnI), new ElementText("\nPowered by 露儿「Yorushika」")));
                            return true;
                        }
                    }
                    if (ele is ElementText && (ele as ElementText).text.ToLower().Contains(keyword2))
                    {
                        WebClient mwc = new WebClient();
                        byte[] nndata = mwc.DownloadData("http://nuannuan.yorushika.co:5000/text/");
                        string nnjson = Encoding.UTF8.GetString(nndata);
                        nnjd nnjsonb = JsonConvert.DeserializeObject<nnjd>(nnjson);
                        await client.SendMessageAsync(me.messageType, srcid,
                            new Message(new ElementText(nnjsonb.content), new ElementText("\nPowered by 露儿「Yorushika」")));
                    }
                }
            }

            return false;
        }
    }

    class nnjd
    {
        public string content { get; set; }
        public bool success { get; set; }
        public bool available { get; set; }
    }
}