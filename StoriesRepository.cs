using System;


namespace monitory997
{
    public static class StoriesRepository
    {
        private static Random r = new Random (997);
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
            int ind = StoriesRepository.r.Next(StoriesRepository.stories.Length);
            return StoriesRepository.stories[ind];
        }
    }
}