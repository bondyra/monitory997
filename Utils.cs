using System;

namespace monitorsProj
{

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