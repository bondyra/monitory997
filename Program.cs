using System;
using System.Threading;

namespace monitory997
{
    public class Knight
    {
        private readonly Tuple<double, double> knightSleepRange = new Tuple<double, double>(5d, 10d);
        private readonly Tuple<double, double> knightSprechenRange = new Tuple<double, double>(1d, 5d);
        
        private bool isKing;
        private int id;

        // Dodac id rycerza
        private readonly Action[] knightFunctions;
        private readonly Monitory monitory;

        public Knight(bool isKing, int id, Monitory monitory)
        {
            this.isKing = isKing;
            this.id = id;
            this.monitory = monitory;
            knightFunctions = new Action[]
            {
                knechtSprechen, knechtTrinken, knechtSchlafen
            };
        }

        public void run()
        {
            Utils.logEvent("Startuje nowy rycerz. Jest krolem: " + isKing);
            for (var i = 0; i < 3; i++)
            {
                Utils.logEvent("Rycerz " + resolveActionName(i));
                monitory.doKnightSprechen(id);
            }
            Utils.logEvent("Rycerz sie najebal "+ id + " i spi pod stolem");
        }

        private String resolveActionName(int position)
        {
            return position == 0 ? "Sprechen" : position == 1 ? "Trinken" : "Schlafen";
        }

        private void knechtTrinken()
        {
            Utils.logEvent("Rycerz pije");
        }

        private void knechtSchlafen()
        {
            var sleepTime = Utils.getRandomMiliseconds(knightSleepRange.Item1, knightSleepRange.Item2);
            Utils.logEvent("Rycerz idzie spac na: " + sleepTime);
            Thread.Sleep(sleepTime);
        }

        public void knechtSprechen()
        {
            var sleepTime = Utils.getRandomMiliseconds(knightSprechenRange.Item1, knightSprechenRange.Item2);
            Utils.logEvent("Rycerz: " + id + "o powiada przez " + sleepTime + " " +  Utils.getRandomStory());
            Thread.Sleep(sleepTime);
        }
    }

    public static class Utils
    {
        private static Random r = new Random(997);
        
        public static void logEvent(String eventText)
        {
            System.Console.WriteLine(System.DateTime.Now + " " + eventText);
        }
        
        public static int getRandomMiliseconds(double min, double max)
        {
            double m = r.NextDouble();
            return (int) (1000 * (min + m * (max - min)));
        }
        
        private static string[] stories = new string[] {
            "Raz sie tak najebalem ze sprzedalem ziomka na komendzie w niedziele z zakazem handlu.",
            "Raz sie tak najebalem ze wstalem rano na Bródnie.",
            "Moja matka nie ma oczu.",
            "Mój poduszkowiec jest pełen węgorzy",
            "Mam jedną pierdoloną schizofrenię",
            "Lecimy tutaj",
            "Tande rucha pande"
        };
        public static string getRandomStory (){
            int ind = r.Next(stories.Length);
            return stories[ind];
        }
    }

    public class Monitory
    {
        private Random r = new Random(997);

        private const int N = 2 * K;
        private const int K = 2;

        private readonly Tuple<double, double> servantSleepRange = new Tuple<double, double>(5d, 10d);
        private readonly Knight[] knights = new Knight[N];
        
        private readonly object[] channelKnights = new object[N];
            
        private readonly int[] knightsStates = new int[N]; // 0 jak nie mówi
        private bool isKingSprechen = false;
        private object globalLock = new object();


        public Monitory()
        {
        }

        public void doKnightSprechen(int id)
        {
            lock (globalLock) // channel pomiedzy Knight a Knight -1
            {
//                lock (channelKnights[(id + 1) % N])// channel pomiedzy Knight a Knigt + 1
//                {
                    knights[id].knechtSprechen();
//                }
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

            while (true)
            {
                
            }

            //słuzacy
//            while (true)
//            {
//                Utils.logEvent("Przychodzi nowy sluzacy.");
//                new Thread(() =>
//                {
//                    Thread.CurrentThread.IsBackground = true;
//                    //wywolac funkcje
//                }).Start();
//                var sleepTime = Utils.getRandomMiliseconds(servantSleepRange.Item1, servantSleepRange.Item2);
//                Utils.logEvent("Do przyjscia kolejnego sluzacego minie: " + sleepTime);
//                Thread.Sleep(sleepTime);
//            }
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