using System;
using System.Threading;

namespace monitorsProj
{
    public class Monitory
    {
        private readonly Knight[] knights = new Knight[Utils.N];

        /*//////////////////////////////////////////////////////*/
        //MONITORY
        //
        //MÓWIENIE
        //synchronizacja mówienia sąsiadów
        private DiningPhilosophersMonitor channelMonitor;
        //synchronizacja nie-mówienia-gdy-król-mówi
        private KingSpeechMonitor kingSpeechMonitor;
        //
        //JEDZENIE
        //synchronizacja talerzy i kielichów
        private DiningPhilosophersMonitor toolsMonitor;
        //synchronizacja dostępu do ogórków/wina
        private ResourcesMonitor resourcesMonitor;
        //prosty monitor - globalna kolejka
        private QueueMonitor diningQueueMonitor;
        /*//////////////////////////////////////////////////////*/

        public Monitory()
        {
            this.channelMonitor = new DiningPhilosophersMonitor(Utils.N);
            this.kingSpeechMonitor = new KingSpeechMonitor();
            this.resourcesMonitor = new ResourcesMonitor(Utils.W, Utils.C, Utils.N);
            this.toolsMonitor = new DiningPhilosophersMonitor(Utils.N);
            this.diningQueueMonitor = new QueueMonitor();
        }

        public void doKnightSpeak(int id)
        {
            var name = Utils.getName(id);
            Utils.logEvent($"{name} sprawdza czy może mówić.");
            if (id!=0) 
                kingSpeechMonitor.WaitIfKingIsSpeaking(name);

            channelMonitor.PickUpForks(id);
            Utils.logEvent($"{name} rezerwuje kanały.");

            //tutaj znowu ustawienie sie w kolejce jeżeli król mówi.
            //to nie jest głupie, bo i tak zanim król nie skończy mówić rycerze nic nie mogą zrobić
            if (id!=0) 
                kingSpeechMonitor.WaitIfKingIsSpeaking(name);
            else
                kingSpeechMonitor.ShutUp(); //król blokuje możliwość mówienia

            knights[id].knightStory();

            channelMonitor.PutDownForks(id);
            Utils.logEvent($"{name} skończył mówić.");
            
            //sekcja dla króla, zwolnienie czekających:
            if (id==0)
            {
                Utils.logEvent($"{name} pozwala rycerzom na mówienie.");
                kingSpeechMonitor.ReleaseKnightQueue();
            }
        }

        //jedzenie przez cykl przechodzący po monitorach
        public void doKnightEat (int id){
            var name = Utils.getName(id);
            var hasDined = false;
            while (!hasDined){
                //jednoczesne podnoszenie talerza i kielicha
                Utils.logEvent($"{name} stara się wziąć kielich i talerz.");
                toolsMonitor.PickUpForks(id);
                Utils.logEvent($"{name} wziął kielich i talerz.");

                //po wzięciu, próbujemy zjeść
                var resourcesExist = resourcesMonitor.TryEat(id); //Item1==isCucumber, Item2==isWine

                //niezależnie od wyniku próby zjedzenia, odkładamy talerz i kielich
                Utils.logEvent($"{name} odkłada kielich i talerz.");
                toolsMonitor.PutDownForks(id);

                //gdy nie udało się zjeść, zasypiamy w 3 monitorze w jednej z dwóch kolejek
                hasDined = resourcesExist.Item1 && resourcesExist.Item2;
                if (!hasDined){
                    diningQueueMonitor.EnterAndWait(id, resourcesExist.Item1, resourcesExist.Item2);
                    Utils.logEvent($"{name} został zwolniony z kolejki głodnych rycerzy.");
                }
            }
        }

        private void wineServantWork (){
            while (true){
                var sleepTime = Utils.getRandomMiliseconds(Utils.ServantSleepRange.Item1, Utils.ServantSleepRange.Item2);
                Utils.logEvent($"Do przyjścia służącego WINO minie {Utils.printTime(sleepTime)}s.");
                Thread.Sleep(sleepTime);

                Utils.logEvent($"Służący uzupełnia WINO.");
                resourcesMonitor.ReplenshWineBottle();
                Utils.logEvent($"Służący WINO zwalnia kolejkę oczekujących na WINO.");
                diningQueueMonitor.EnterAndRelease(true);
            }
        }

        private void cucumberServantWork(){
            while (true){
                var sleepTime = Utils.getRandomMiliseconds(Utils.ServantSleepRange.Item1, Utils.ServantSleepRange.Item2);
                Utils.logEvent($"Do przyjścia służącego OGÓRKI minie {Utils.printTime(sleepTime)}s.");
                Thread.Sleep(sleepTime);

                Utils.logEvent($"Służący uzupełnia OGÓRKI.");
                resourcesMonitor.ReplenishCucumberPlates();
                Utils.logEvent($"Służący OGÓRKI zwalnia kolejkę oczekujących na OGÓRKI.");
                diningQueueMonitor.EnterAndRelease(false);
            }
        }

        public void run()
        {
            Utils.logEvent("Zaczynamy.");
            //rycerze
            for (int i = 0; i < Utils.N; i++)
            {
                var i1 = i;
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    knights[i1] = new Knight(i1 == 0, i1 , this);
                    knights[i1].run();
                }).Start();
            }

            //służący
            new Thread( () => {
                Thread.CurrentThread.IsBackground = true;
                wineServantWork();
            }).Start();
            new Thread( () => {
                Thread.CurrentThread.IsBackground = true;
                cucumberServantWork();
            }).Start();

            //by nie zabić programu
            while (true){}
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var monitory = new Monitory();
            monitory.run();
        }
    }
}