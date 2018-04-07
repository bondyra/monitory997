using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace monitorsProj
{
    public class ResourcesMonitor
    {
        private int N;
        //butelka
        private int wine;
        private int w;
        private int c;
        //talerzyki
        private int[] resources;
        private object monitorObject = new object();

        
        private ConditionVariable[] knightWaiting;
        private List<int> waitersQueue;


        public ResourcesMonitor(int _w, int _c, int _N)
        {
            N = _N;
            c = _c;
            resources = new int[N];
            for (int i=0;i<N;i++){  
                resources[i] = _c;
            }
            wine = _w;
            w = _w;
            waitersQueue = new List<int>();
            knightWaiting = new ConditionVariable[N];
            for (int i=0;i<N;i++){
                knightWaiting[i] = new ConditionVariable();
            }
            monitorObject = new object();
        }

        //entries
        public void wineServantWork(){
            lock (monitorObject){
                Utils.logEvent("Służący uzupełnia WINO.");
                wine = w;
                Utils.logEvent("Służący WINO budzi pierwszego rycerza z kolejki."); 
                awakeFirstFromWaitingQueue();
            }
        }

        public void cucumberServantWork(){
            lock (monitorObject){
                Utils.logEvent("Służący uzupełnia OGÓRKI."); 
                for (int i=0;i<N;i+=2)
                    resources[i] = c;
                Utils.logEvent("Służący OGÓRKI budzi pierwszego rycerza z kolejki."); 
                awakeFirstFromWaitingQueue();
            }
        }

        public void knightEntryWork(int id){
            var name = Utils.getName(id);
            int cid = resolveCucumber (id);
            //rycerz stara sie zajac swoje zasoby - jezeli nie moze - czeka
            lock (monitorObject){
                if (resources[cid] == 0 || wine == 0){
                    Utils.logEvent($"{name} nie ma zasobów więc czeka."); 
                    waitersQueue.Add(id);
                    knightWaiting[id].Wait(monitorObject);
                    Utils.logEvent($"{name} został obudzony."); 
                }

                if (resources[cid] == 0 || wine == 0)
                    throw new Exception("Coś poszło zupełnie nie tak.");

                resources[cid]--;
                wine--;
                Utils.logEvent($"{name} budzi pierwszego rycerza z kolejki."); 
                awakeFirstFromWaitingQueue();
            }
        }

        //zalążek pracy doktorskiej inż. Michała Berenta
        private int resolveCucumber (int id){
            return (id + id%2)%N;
        }
        private int resolveWine (int id){
            return id - id%2 + 1;
        }

        private void awakeFirstFromWaitingQueue (){
            logResources();
            foreach (var waiter in waitersQueue){
                if (canBeAwaken(waiter))
                {
                    var name = Utils.getName(waiter);
                    Utils.logEvent($"Budzony jest {name}.");
                    waitersQueue.Remove(waiter);
                    knightWaiting[waiter].Pulse();
                    return;
                }
            }
        }

        //odpalane wewnatrz monitora!
        private bool canBeAwaken (int id){
            int wid = resolveWine (id);
            int cid = resolveCucumber (id);
            return resources[wid] > 0 && resources[cid] > 0 && wine > 0;
        }
        
        //podobnie jak wcześniej z alchemikami, logowanie stanu zasobów
        private void logResources (){
            Utils.logEvent($"STAN\nKARAFKA: {wine}, \nRESOURCES:{resourcesToString()}\nWAITERS:{waitersToString()}");
        }

        //przykłady złego stylu programowania :)
        private string resourcesToString(){
            var sb = new StringBuilder();
            sb.Append("[");
            for (int i=0;i<resources.Length;i++)
                sb.Append(resources[i]+",");
            sb.Append("]");
            return sb.ToString();
        }
        private string waitersToString(){
            var sb = new StringBuilder();
            sb.Append("[");
            for (int i=0;i<waitersQueue.Count;i++)
                sb.Append(Utils.getName(waitersQueue[i])+",");
            sb.Append("]");
            return sb.ToString();
        }
    }
}