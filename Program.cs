using System;
using System.Threading;

namespace monitory997
{
    public class Monitory
    {
        private Random r = new Random(997);

        private const int N = 2 * K;
        private const int K = 2;

        private readonly Tuple<double, double> servantSleepRange = new Tuple<double, double>(5d, 10d);
        private readonly Knight[] knights = new Knight[N];
        
        //monitory w oddzielnych klasach:
        private ChannelMonitor channelMonitor;
        private KingSpeechMonitor kingSpeechMonitor;

        public Monitory()
        {
            this.channelMonitor = new ChannelMonitor(N);
            this.kingSpeechMonitor = new KingSpeechMonitor();
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
                kingSpeechMonitor.ShutTheFuckUp(); //król blokuje możliwość mówienia

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

        public void run()
        {
            Utils.logEvent("Hello Monitory997!");
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

            //słuzacy
            while (true)
            {
                Utils.logEvent("Przychodzi nowy służący.");
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    //wywolac funkcje
                }).Start();
                var sleepTime = Utils.getRandomMiliseconds(servantSleepRange.Item1, servantSleepRange.Item2);
                Utils.logEvent($"Do przyjścia kolejnego slużącego minie {Utils.printTime(sleepTime)}s.");
                Thread.Sleep(sleepTime);
            }
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