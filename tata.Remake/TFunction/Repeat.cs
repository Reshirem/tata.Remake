using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;
using cqhttp.Cyan.Messages.CQElements.Base;

namespace tata.Remake.TFunction
{
    public class Repeat : IMessageProcess
    {
        Random rand_ = new Random();
        private int repeatCounter_;
        private string[] repeatHash_ = new string[500];
        private long[] repeatTime_ = new long[500];
        private bool[] repeatAlready_ = new bool[500];
        
        public async Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            string rHash = Global.GetMd5Hash(MD5.Create(), me.message.ToString());

            int[] repeatlist_ = new int[10];
            int repeatlistCounter_ = 0;
            for (int i = 0; i < 500; i++)
            {
                if (repeatHash_[i] == rHash)
                {
                    if (repeatTime_[i] + 60 > me.time)
                    {
                        repeatlist_[repeatlistCounter_] = i;
                        repeatlistCounter_ = repeatlistCounter_ + 1;
                    }

                    foreach (var rlist in repeatlist_)
                    {
                        if (repeatAlready_[rlist])
                        {
                            return false;
                        }

                        if (rlist == 0)
                        {
                            break;
                        }
                    }
                }
            }

            if (repeatlistCounter_ > 2)
            {
                int rroll = rand_.Next(1, 100);
                if (rroll < 90)
                {
                    await client.SendMessageAsync(me.messageType, srcid, me.message);
                    repeatHash_[repeatCounter_] = rHash;
                    repeatTime_[repeatCounter_] = me.time;
                    repeatAlready_[repeatCounter_] = true;
                    await client.SendMessageAsync(me.messageType, srcid,
                        new Message(new ElementText($"{repeatAlready_[4]}, {repeatCounter_}")));
                    repeatCounter_ = repeatCounter_ + 1;
                    if (repeatCounter_ > 500)
                    { 
                        repeatCounter_ = 0;
                    }

                    return true;
                }

                repeatHash_[repeatCounter_] = rHash; 
                repeatTime_[repeatCounter_] = me.time;
                repeatAlready_[repeatCounter_] = false;
                repeatCounter_ = repeatCounter_ + 1;
                if (repeatCounter_ > 500)
                { 
                    repeatCounter_ = 0;
                }

                return true;
            }
            repeatHash_[repeatCounter_] = rHash; 
            repeatTime_[repeatCounter_] = me.time;
            repeatAlready_[repeatCounter_] = false;
            repeatCounter_ = repeatCounter_ + 1;
            if (repeatCounter_ > 500)
            { 
                repeatCounter_ = 0;
            }

            return false;
        }
    }
}