using System;

namespace monitorsProj
{
    //ogólny monitor do komunikacji oraz problemu kielichy+talerze
    //dokładnie jak na wykładzie (ucztujący filozofowie)
    public class DiningPhilosophersMonitor
    {
        //obiekt na którym zakładany jest lock
        private readonly object monitorLock = new object();

        //==QWait:
        private readonly ConditionVariable[] knightQueue;
        //czy widelec jest zajety? (==Fork)
        private readonly bool[] forkTaken = new bool[6];
        //czy rycerz czeka na widelce? (==Wait)
        private readonly bool[] knightWaiting = new bool[6];
        //potrzebne do budzenia 
        private bool wait = false;

        private readonly int N;

        public DiningPhilosophersMonitor(int N) 
        {
            this.N = N;
            knightQueue = new ConditionVariable[N];
            for (int i=0;i<knightQueue.Length;i++)
                knightQueue[i] = new ConditionVariable();
            forkTaken = new bool[N];
            knightWaiting = new bool[N];
        }

        //ENTRIES

        public void PickUpForks (int id)
        {
            lock (monitorLock)
            {
                if (forkTaken[id] || forkTaken[(id+1)%N])
                {
                    knightWaiting[id] = true;
                    Utils.logEvent($"{Utils.getName(id)} nie może zająć zasobów, czeka.");
                    knightQueue[id].Wait(monitorLock);
                    knightWaiting[id] = false;
                }
                forkTaken[id] = true;
                forkTaken[(id+1)%N] = true;
                if (wait)
                {
                    wait = false;
                    knightQueue[(N+id-2)%N].Pulse();
                }
            }
        }

        public void PutDownForks (int id)
        {
            lock (monitorLock)
            {
                forkTaken[id] = false;
                forkTaken[(id+1)%N] = false;
                if (knightWaiting[(N+id-1)%N] && !forkTaken[(N+id-1)%N])
                    wait = true;
                if (knightWaiting[(id+1)%N] && !forkTaken[(id+2)%N]){
                    knightQueue[(id+1)%N].Pulse();
                    return;
                }
                if (wait)
                {
                    wait = false;
                    knightQueue[(N+id-1)%N].Pulse();
                }
            }
        }
    }
}