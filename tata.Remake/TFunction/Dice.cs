using System;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace tata.Remake.TFunction
{
    public class Dice : IMessageProcess
    {
        Random rand_ = new Random();

        async public Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            const string keyword = "/dice";
            bool isGroup = me is cqhttp.Cyan.Events.CQEvents.GroupMessageEvent;
            if (Global.msgFilter(me.message, false, s => s.TrimStart().StartsWith(keyword)))
            {
                foreach (var ele in me.message.data)
                {
                    if (ele is ElementText && (ele as ElementText).text.ToLower().Contains('d'))
                    {
                        try
                        {
                            string txt = (ele as ElementText).text.TrimStart();
                            if (!txt.StartsWith(keyword))
                                continue;

                            string[] sa = txt.Split(new[] {"d", keyword}, StringSplitOptions.RemoveEmptyEntries);
                            if (sa.Length == 2)
                            {
                                int cnt = int.Parse(sa[0]);
                                int rng = int.Parse(sa[1]);
                                
                                if (cnt == 0)
                                {
                                    await client.SendMessageAsync(me.messageType, srcid,
                                        new Message(new ElementAt(me.sender.user_id),
                                            new ElementText(" wdnmd，你不用骰子还摇个鸡掰？")));
                                    break;
                                }
                                
                                if (cnt < 0)
                                {
                                    await client.SendMessageAsync(me.messageType, srcid,
                                        new Message(new ElementAt(me.sender.user_id),
                                            new ElementText(" wdnmd，你是在用虚空骰子？")));
                                    break;
                                }
                                
                                if (rng < 1)
                                {
                                    await client.SendMessageAsync(me.messageType, srcid,
                                        new Message(new ElementAt(me.sender.user_id),
                                            new ElementText(" wdnmd，你那个是骰子？")));
                                    break;
                                }
                                
                                if (cnt > 32)
                                {
                                    await client.SendMessageAsync(me.messageType, srcid,
                                        new Message(new ElementAt(me.sender.user_id),
                                            new ElementText(" wdnmd，你哪有那么多骰子？")));
                                    break;
                                }

                                if (rng > 999)
                                {
                                    await client.SendMessageAsync(me.messageType, srcid,
                                        new Message(new ElementAt(me.sender.user_id),
                                            new ElementText(" wdnmd，哪有那么多面的骰子？")));
                                    break;
                                }
                                    
                                int[] result = new int[cnt];
                                for (int i = 0; i < cnt; i++)
                                    result[i] = rand_.Next(rng) + 1;
                                string ans = string.Join("，", result);
                                
                                await client.SendMessageAsync(me.messageType, srcid, new Message(new ElementAt(me.sender.user_id) , new ElementText($" 转动{cnt}个{rng}面骰子，掷出了[ {ans} ]。")));
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                }
            }

            return false;
        }
    }
}