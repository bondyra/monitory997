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
        private ConditionVariable queue = new ConditionVariable();
        //wyłącznie do ładnych logów:
        private List<int> knightIds = new List<int>();

        //ENTRIES

        //rycerz wchodzi i czeka
        public void EnterAndWait(int id){
            lock (monitorObject){
                knightIds.Add(id);
                queue.Wait(monitorObject);
                knightIds.Remove(id);
            }
        }
        
        //slużący wchodzi i zwalnia pulseAll
        public void EnterAndRelease(){
            lock(monitorObject){
                Utils.logEvent($"ZWALNIANA KOLEJKA GŁODNYCH: [{String.Join(",",knightIds.Select(id => Utils.getName(id)))}]");
                queue.PulseAll();
            }
        }

    }
}