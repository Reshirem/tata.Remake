using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cqhttp.Cyan.ApiCall.Requests;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace tata.Remake.TFunction
{
    public class Lottery : IMessageProcess
    {
        private bool islbing;
        private long lbqun;
        private long cts = 0;
        List<long> currentList_ = new List<long>();
        Random rand_ = new Random();
        public async Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            const string keyword = "/lotteryban";
            const string keyword1 = "/lob";
            double dts = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
            long bts = (long) dts;
            if (islbing && cts + 420 < bts)
            {
                islbing = false;
                currentList_.Clear();
            }
            bool isGroup = me is cqhttp.Cyan.Events.CQEvents.GroupMessageEvent;
            if (Global.msgFilter(me.message, !isGroup, s => s.TrimStart().StartsWith(keyword)))
            {
                if (islbing)
                {
                    await client.SendMessageAsync(me.messageType, srcid, new Message(new ElementText("已经有禁言抽奖正在进行咯！\n（或许不是这个群的）")));
                    return true;
                }

                if (!islbing)
                {
                    islbing = true;
                    lbqun = srcid;
                    currentList_.Add(me.sender.user_id);
                    double ats = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
                    cts = (long) ats;
                    await client.SendMessageAsync(me.messageType, srcid, new Message(new ElementAt(me.sender.user_id),
                        new ElementText(" 发起了1分钟的禁言抽奖（1/5）\n" +
                                        "各位快输入「/lob」参与吧！(7分钟后没达到人数自动关闭)")));
                    return true;
                }
            }

            if (Global.msgFilter(me.message, !isGroup, s => s.TrimStart().StartsWith(keyword1)))
            {
                if (islbing && srcid == lbqun)
                {
                    foreach (var qqid in currentList_)
                    {
                        if (qqid == me.sender.user_id)
                        {
                            await client.SendMessageAsync(me.messageType, srcid,
                                new Message(new ElementAt(me.sender.user_id), new ElementText(" 你已经在名单里了哦~")));
                            return false;
                        }
                    }
                    currentList_.Add(me.sender.user_id);
                    if (currentList_.Count == 5)
                    {
                        int rroll0 = rand_.Next(1, 100);
                        int rroll1 = rand_.Next(1, 100);
                        int rroll2 = rand_.Next(1, 100);
                        int rroll3 = rand_.Next(1, 100);
                        int rroll4 = rand_.Next(1, 100);
                        int winnum = 0;
                        

                        for (int i = 100; i > 1; i--)
                        {
                            if (rroll0 == i)
                            {
                                winnum = 0;
                                break;
                            }

                            if (rroll1 == i)
                            {
                                winnum = 1;
                                break;
                            }

                            if (rroll2 == i)
                            {
                                winnum = 2;
                                break;
                            }

                            if (rroll3 == i)
                            {
                                winnum = 3;
                                break;
                            }

                            if (rroll4 == i)
                            {
                                winnum = 4;
                                break;
                            }
                        }

                        await client.SendMessageAsync(me.messageType, srcid,
                            new Message(new ElementText("禁言1分钟抽奖开奖啦！\n"),
                                new ElementAt(currentList_[0]), new ElementText($" 的点数为[{rroll0}]\n"),
                                new ElementAt(currentList_[1]), new ElementText($" 的点数为[{rroll1}]\n"),
                                new ElementAt(currentList_[2]), new ElementText($" 的点数为[{rroll2}]\n"),
                                new ElementAt(currentList_[3]), new ElementText($" 的点数为[{rroll3}]\n"),
                                new ElementAt(currentList_[4]), new ElementText($" 的点数为[{rroll4}]\n"),
                                new ElementText("点数最大的是："), new ElementAt(currentList_[winnum]),
                                new ElementText("恭喜这位群友获得1分钟禁言礼包~")));
                        await client.SendRequestAsync(new SetGroupBanRequest(srcid, currentList_[winnum], 60L));
                        islbing = false;
                        currentList_.Clear();
                        return true;
                    }
                    await client.SendMessageAsync(me.messageType, srcid,
                        new Message(new ElementAt(me.sender.user_id),
                            new ElementText($" 也参与到禁言抽奖啦！（{currentList_.Count}/5）\n" + "还没有参与的群友们赶紧输入「/lob」参与吧！")));
                    return true;
                }

                if (!islbing)
                {
                    await client.SendMessageAsync(me.messageType, srcid,
                        new Message(new ElementText("目前还没有禁言抽奖活动进行哦！")));
                    return true;
                }
            }
            
            return false;
        }
    }
}