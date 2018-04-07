using System;

namespace monitorsProj
{
    //symulacja monitora do komunikacji między sąsiadami
    //dokładnie jak na wykładzie (ucztujący filozofowie)
    public class ChannelMonitor
    {
        //obiekt na którym zakładany jest lock
        private readonly object channelLock = new object();
        //==QWait:
        private readonly ConditionVariable[] channelQueue = new ConditionVariable[] {
            new ConditionVariable(), new ConditionVariable(), new ConditionVariable(),
            new ConditionVariable(), new ConditionVariable(), new ConditionVariable()
        };
        //czy kanal jest zajety? (==Fork)
        private readonly bool[] channelTaken = new bool[6];
        //czy rycerz czeka na kanaly? (==Wait)
        private readonly bool[] knightChannelWait = new bool[6];
        //"a flag indicating that the previous neighbor can be awaken"
        private bool wait = false;

        private readonly int N;

        public ChannelMonitor(int N) 
        {
            this.N = N;
        }

        //ENTRIES

        public void TakeChannels (int id)
        {
            lock (channelLock)
            {
                if (channelTaken[id] || channelTaken[(id+1)%N])
                {
                    knightChannelWait[id] = true;
                    channelQueue[id].Wait(channelLock);
                    knightChannelWait[id] = false;
                }
                channelTaken[id] = true;
                channelTaken[(id+1)%N] = true;
                if (wait)
                {
                    wait = false;
                    channelQueue[(N+id-2)%N].Pulse();
                }
            }
        }

        public void ReleaseChannels (int id)
        {
            lock (channelLock)
            {
                channelTaken[id] = false;
                channelTaken[(id+1)%N] = false;
                if (knightChannelWait[(N+id-1)%N] && !channelTaken[(N+id-1)%N])
                    wait = true;
                if (knightChannelWait[(id+1)%N] && !channelTaken[(id+2)%N]){
                    channelQueue[(id+1)%N].Pulse();
                    return;
                }
                if (wait)
                {
                    wait = false;
                    channelQueue[(N+id-1)%N].Pulse();
                }
            }
        }
    }
}