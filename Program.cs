using System;
using System.Threading;

namespace monitorsProj
{
    public class Monitory
    {
        private Random r = new Random(997);

        private const int N = 2 * K;
        private const int K = 2;
        private const int W = 2;
        private const int C = 2;

        private readonly Tuple<double, double> servantSleepRange = new Tuple<double, double>(5d, 10d);
        private readonly Knight[] knights = new Knight[N];
        
        //monitory w oddzielnych klasach:
        private ChannelMonitor channelMonitor;
        private KingSpeechMonitor kingSpeechMonitor;

        private ResourcesMonitor resourcesMonitor;

        public Monitory()
        {
            this.channelMonitor = new ChannelMonitor(N);
            this.kingSpeechMonitor = new KingSpeechMonitor();
            this.resourcesMonitor = new ResourcesMonitor(W, C, N);
        }

        public void doKnightSpeak(int id)
        {
            var name = Utils.getName(id);
            if (id!=0) 
                kingSpeechMonitor.WaitIfKingIsSpeaking(name);
            //"try-to-take-forks"
            Utils.logEvent($"{name} sprawdza czy może mówić.");
            channelMonitor.TakeChannels(id);

            Utils.logEvent($"{name} zajął kanały {id} i {(id+1)%N}.");

            //tutaj znowu ustawienie sie w kolejce jeżeli król mówi.
            //to nie jest głupie, bo i tak zanim król nie skończy mówić rycerze nic nie mogą zrobić
            if (id!=0) 
                kingSpeechMonitor.WaitIfKingIsSpeaking(name);
            else
                kingSpeechMonitor.ShutUp(); //król blokuje możliwość mówienia

            knights[id].knightStory();
            //"put-down-forks"
            Utils.logEvent($"{name} skończył mówić.");
            channelMonitor.ReleaseChannels(id);
            //sekcja dla króla, zwolnienie czekających:
            if (id==0)
            {
                Utils.logEvent($"{name} zwalnia kolejkę rycerzy.");
                kingSpeechMonitor.ReleaseKnightQueue();
            }
        }

        public void doKnightEat (int id){
            resourcesMonitor.knightEntryWork(id);
        }

        private void wineServantWork (){
            while (true){
                var sleepTime = Utils.getRandomMiliseconds(servantSleepRange.Item1, servantSleepRange.Item2);
                Utils.logEvent($"Do przyjścia służącego WINO minie {Utils.printTime(sleepTime)}s.");
                Thread.Sleep(sleepTime);
                resourcesMonitor.wineServantWork();
            }
        }

        private void cucumberServantWork(){
            while (true){
                var sleepTime = Utils.getRandomMiliseconds(servantSleepRange.Item1, servantSleepRange.Item2);
                Utils.logEvent($"Do przyjścia służącego OGÓRKI minie {Utils.printTime(sleepTime)}s.");
                Thread.Sleep(sleepTime);
                resourcesMonitor.cucumberServantWork();
            }
        }

        public void run()
        {
            Utils.logEvent("Zaczynamy.");
            //rycerze
            for (int i = 0; i < N; i++)
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