using System;
using System.Text;

namespace monitorsProj
{

    public static class Utils
    {
        private static Random r = new Random(997);        
        
        //stałe programu
        public static readonly Tuple<double, double> ServantSleepRange = new Tuple<double, double>(0.5d, 1d);
        public static readonly Tuple<double, double> KnightSleepRange = new Tuple<double, double>(0.5d, 0.10d);
        public static readonly Tuple<double, double> KnightSpeakRange = new Tuple<double, double>(0.1d, 0.5d);
        public static readonly int KnightRepeats = 10;
        
        public static readonly int K = 3;
        public static readonly int N = 2 * K;
        //max pojemnosc karafki wina
        public static readonly int W = 2;
        //max pojemnosc talerzy ogorkow
        public static readonly int C = 2;
        
        public static void logEvent(String eventText)
        {
            System.Console.WriteLine(System.DateTime.Now + " " + eventText);
        }
        
        public static int getRandomMiliseconds(double min, double max)
        {
            double m = r.NextDouble();
            return (int) (1000 * (min + m * (max - min)));
        }

        public static string printTime (int time){
            return Math.Round(((double)time)/1000f, 2).ToString();
        }

        public static string getName (int id){
            return id==0? "*KRÓL*" : $"Rycerz {id}";
        }
        
        private static string[] stories = new string[] {
            "Litwo! Ojczyzno moja! Ty jesteś jak zdrowie!",
            "Apator Toruń pije wodę z Wisły.", "Nadczłowiek jest treścią ziemi.", "Lorem ipsum dolor sit amet.",
            "Podaję hasło: Okoń.", "Kiedyś to było.", "Mój poduszkowiec jest pełen węgorzy.",
            "Randomowa historia #1.", "Randomowa historia #2.", "Randomowa historia #3."
        };
        public static string getRandomStory (){
            int ind = r.Next(stories.Length);
            return stories[ind];
        }

        public static bool getRandomBool (){
            return r.Next(0,1) == 0 ? false : true;
        }        
    }
}