using System;
using System.Threading;

namespace monitorsProj
{
    public class Knight
    {
        private const int repeats = 3;
        private readonly Tuple<double, double> knightSleepRange = new Tuple<double, double>(5d, 10d);
        private readonly Tuple<double, double> knightSpeakRange = new Tuple<double, double>(1d, 5d);
        
        private bool isKing;
        private int id;
        private string name;

        private readonly Action[] knightFunctions;
        private readonly Monitory monitory;

        public Knight(bool isKing, int id, Monitory monitory)
        {
            this.isKing = isKing;
            this.id = id;
            this.name = Utils.getName(id);
            this.monitory = monitory;
            knightFunctions = new Action[]
            {
                knightSpeaks, knightDrinks, knightSleeps
            };
        }

        public void run()
        {
            Utils.logEvent($"Startuje {name}.");
            for (int j=0;j<repeats;j++){
                for (var i = 0; i < knightFunctions.Length; i++)
                {
                    Utils.logEvent($"{name} rozpoczyna: {resolveActionName(i)} ({j+1})");
                    knightFunctions[i].Invoke();
                }
            }
            Utils.logEvent($"{name} spadł pod stół i zasypia na amen. Już nie wróci.");
        }

        private String resolveActionName(int position)
        {
            return position == 0 ? "OPOWIADANIE" : position == 1 ? "PICIE" : "SPANIE";
        }

        private void knightDrinks()
        {
            monitory.doKnightEat(id);
        }

        private void knightSleeps()
        {
            var sleepTime = Utils.getRandomMiliseconds(knightSleepRange.Item1, knightSleepRange.Item2);
            Utils.logEvent($"{name} idzie spać na {Utils.printTime(sleepTime)}s");
            Thread.Sleep(sleepTime);
        }

        private void knightSpeaks()
        {
            monitory.doKnightSpeak(id);
        }

        public void knightStory()
        {
            var sleepTime = Utils.getRandomMiliseconds(knightSpeakRange.Item1, knightSpeakRange.Item2);
            Utils.logEvent($"{name} opowiada przez {Utils.printTime(sleepTime)}s:  {Utils.getRandomStory()}");
            Thread.Sleep(sleepTime);
        }
    }
}