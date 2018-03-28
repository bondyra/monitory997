using System;

namespace monitory997
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
            "Raz sie tak najebalem ze sprzedalem ziomka na komendzie w niedziele z zakazem handlu.",
            "Raz sie tak najebalem ze wstalem rano na Bródnie.","Jony to debil.","Ja to pierdolę, zdejmuję kominiarę.",
            "Moja matka nie ma oczu.","Kiedyś to było.","Grunwald i Jeżyce to dwie najbardziej pojebane dzielnice.",
            "Mój poduszkowiec jest pełen węgorzy.","Liroy Liroy ten jebany madafaka...","Pozdro 600.",
            "Mam jedną pierdoloną schizofrenię.","Gdy wydarzy się incydent to pojawia się konfident...",
            "Lecimy tutaj.", "Tande rucha pande.", "Jak tak patrzę na was to chce mi się srać."
        };
        public static string getRandomStory (){
            int ind = r.Next(stories.Length);
            return stories[ind];
        }
    }
}