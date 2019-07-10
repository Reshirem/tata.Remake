using System;
using System.Threading.Tasks;
using cqhttp.Cyan.ApiCall.Requests;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace tata.Remake.TFunction
{
    public class Autistic : IMessageProcess
    {
        public async Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            const string keyword0 = "管理员";
            const string keyword1 = "狗管理";
            const string keyword2 = "来一份";
            const string keyword3 = "禁言";
            const string keyword4 = "分";
            const string keyword5 = "小时";
            const string keyword6 = "天";
            bool isGroup = me is cqhttp.Cyan.Events.CQEvents.GroupMessageEvent;
            if (Global.msgFilter(me.message, !isGroup, s => s.TrimStart().StartsWith(keyword0)) || 
                Global.msgFilter(me.message, !isGroup, s => s.TrimStart().StartsWith(keyword1)))
            {
                if (Global.msgFilter(me.message, !isGroup, s => s.TrimStart().Contains(keyword2)) &&
                    Global.msgFilter(me.message, !isGroup, s => s.TrimStart().Contains(keyword3)))
                {
                    foreach (var ele in me.message.data)
                    {
                        if (ele is ElementText)
                        {
                            if ((ele as ElementText).text.Contains(keyword4) || (ele as ElementText).text.Contains(keyword5) || (ele as ElementText).text.Contains(keyword6))
                            {
                                string txt = (ele as ElementText).text.TrimStart();
                                int k2leng = txt.IndexOf(keyword2);
                                string txt0 = txt.Remove(0, k2leng + 3);
                                string txt1 = "0";
                                string txt2 = null;
                                int btype = 0;
                                long aleng = 0; 
                                long aleng0 = 0;
                                if (txt0.Contains(keyword4) && !txt0.Contains(keyword5) &&
                                    !txt0.Contains(keyword6))
                                {
                                    btype = 1;
                                    int k4leng = txt0.IndexOf(keyword4);
                                    txt1 = txt0.Remove(k4leng);
                                }else if (!txt0.Contains(keyword4) && txt0.Contains(keyword5) &&
                                          !txt0.Contains(keyword6))
                                {
                                    btype = 2;
                                    int k5leng = txt0.IndexOf(keyword5);
                                    txt1 = txt0.Remove(k5leng);
                                }else if (!txt0.Contains(keyword4) && !txt0.Contains(keyword5) &&
                                          txt0.Contains(keyword6))
                                {
                                    btype = 3;
                                    int k6leng = txt0.IndexOf(keyword6);
                                    txt1 = txt0.Remove(k6leng);
                                }else if (txt0.Contains(keyword4) && txt0.Contains(keyword5) &&
                                          !txt0.Contains(keyword6))
                                {
                                    await client.SendMessageAsync(me.messageType, srcid,
                                        new Message(new ElementAt(me.sender.user_id), new ElementText(" 暂时不支持同时定义分钟和小时")));
                                }else if (!txt0.Contains(keyword4) && txt0.Contains(keyword5) &&
                                          txt0.Contains(keyword6))
                                {
                                    await client.SendMessageAsync(me.messageType, srcid,
                                        new Message(new ElementAt(me.sender.user_id), new ElementText(" 暂时不支持同时定义小时和天数")));
                                }else if (txt0.Contains(keyword4) && !txt0.Contains(keyword5) &&
                                          txt0.Contains(keyword6))
                                {
                                    await client.SendMessageAsync(me.messageType, srcid,
                                        new Message(new ElementAt(me.sender.user_id), new ElementText(" 暂时不支持同时定义分钟和天数")));
                                }else if (txt0.Contains(keyword4) && txt0.Contains(keyword5) &&
                                          txt0.Contains(keyword6))
                                {
                                    await client.SendMessageAsync(me.messageType, srcid,
                                        new Message(new ElementAt(me.sender.user_id), new ElementText(" 暂时不支持同时定义分钟和小时和天数")));
                                }

                                if (btype == 0)
                                {
                                    return false;
                                }
                                
                                if (btype == 1)
                                {
                                    try
                                    {
                                        aleng0 = long.Parse(txt1);
                                        if (aleng0 > 43200)
                                        {
                                            aleng0 = 43200;
                                        }

                                        if (aleng0 < 0)
                                        {
                                            btype = 4;
                                        }
                                        aleng = aleng0 * 60;
                                        txt2 = "分钟";
                                    }
                                    catch (Exception)
                                    {
                                        btype = 4;
                                    }
                                }else if (btype == 2)
                                {
                                    try
                                    {
                                        aleng0 = long.Parse(txt1);
                                        if (aleng0 > 720)
                                        {
                                            aleng0 = 720;
                                        }

                                        if (aleng0 < 0)
                                        {
                                            btype = 4;
                                        }
                                        aleng = aleng0 * 60 * 60;
                                        txt2 = "小时";
                                    }
                                    catch (Exception)
                                    {
                                        btype = 4;
                                    }
                                }else if (btype == 3)
                                {
                                    try
                                    {
                                        aleng0 = long.Parse(txt1);
                                        if (aleng0 > 30)
                                        {
                                            aleng0 = 30;
                                        }

                                        if (aleng0 < 0)
                                        {
                                            btype = 4;
                                        }
                                        aleng = aleng0 * 60 * 60 * 24;
                                        txt2 = "天";
                                    }
                                    catch (Exception)
                                    {
                                        btype = 4;
                                    }
                                }

                                if (btype == 4)
                                {
                                    aleng = 3600;
                                    await client.SendMessageAsync(me.messageType, srcid,
                                        new Message(new ElementAt(me.sender.user_id), new ElementText($" 居然调戏我w，先咬死！")));
                                    await client.SendRequestAsync(new SetGroupBanRequest(srcid, me.sender.user_id, aleng));
                                    return true;
                                }

                                await client.SendMessageAsync(me.messageType, srcid,
                                    new Message(new ElementAt(me.sender.user_id), new ElementText($" 您点的{aleng0}{txt2}禁言套餐已到，请尽情享用")));
                                await client.SendRequestAsync(new SetGroupBanRequest(srcid, me.sender.user_id, aleng));
                                return true;
                            }
                        }
                    }
                }
            }
            
            return false;
        }
    }
}