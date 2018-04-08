using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace monitorsProj
{
    //monitor do kontroli nad zasobami
    public class QueueMonitor
    {
        //stałe:
        private readonly int peopleNumber;
        private readonly int cucumberCount;
        private readonly int wineVolume;

        //monitor
        private object monitorObject = new object();

        //chronione przez monitor:
        private ConditionVariable[] queues = {new ConditionVariable(), new ConditionVariable()};
        //wyłącznie do ładnych logów:
        private List<int>[] knightIds = {new List<int>(), new List<int>()};

        //ENTRIES

        //rycerz wchodzi i czeka
        public void EnterAndWait(int id, bool isCucumber, bool isWine){
            //wybór kolejki w zaleznosci od braku
            int qid = -1;
            System.Diagnostics.Debug.Assert(!(isCucumber && isWine));

            if (!isCucumber && !isWine)
                qid = Utils.getRandomBool() ? Utils.CucumberQueue : Utils.WineQueue;
            else if (!isCucumber)
                qid = Utils.CucumberQueue;
            else
                qid = Utils.WineQueue;
            Utils.logEvent($"{Utils.getName(id)} ustawia się w kolejce oczekujących na {(qid==Utils.WineQueue?"WINO":"OGÓRKI")}.");
            lock (monitorObject){
                knightIds[qid].Add(id);
                queues[qid].Wait(monitorObject);
                knightIds[qid].Remove(id);
            }
        }
        
        //slużący wchodzi i zwalnia pulseAll odpowiednią kolejkę
        public void EnterAndRelease(bool wine){
            int qid = wine ? Utils.WineQueue : Utils.CucumberQueue;
            lock(monitorObject){
                var descr = (wine?"WINO":"OGÓRKI");
                if (knightIds[qid].Count>0)
                    Utils.logEvent($"ZWALNIANA KOLEJKA OCZEKUJĄCYCH NA {descr}: [{String.Join(",",knightIds[qid].Select(id => Utils.getName(id)))}]");
                else
                    Utils.logEvent($"Kolejka oczekujących na {descr} jest pusta.");
                queues[qid].PulseAll();
            }
        }

    }
}