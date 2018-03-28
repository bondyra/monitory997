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
        private string name;

        // Dodac id rycerza
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
            for (var i = 0; i < 3; i++)
            {
                Utils.logEvent($"{name} rozpoczyna: {resolveActionName(i)}");
                knightFunctions[i].Invoke();
            }
            Utils.logEvent($"{name} się najebał i śpi pod stołem. Już nie wróci.");
        }

        private String resolveActionName(int position)
        {
            return position == 0 ? "OPOWIADANIE" : position == 1 ? "PICIE" : "SRANIE";
        }

        private void knightDrinks()
        {
            Utils.logEvent("Rycerz pije. TODO.");
        }

        private void knightSleeps()
        {
            var sleepTime = Utils.getRandomMiliseconds(knightSleepRange.Item1, knightSleepRange.Item2);
            Utils.logEvent($"{name} idzie srać na {Utils.printTime(sleepTime)}s");
            Thread.Sleep(sleepTime);
        }

        private void knightSpeaks()
        {
            monitory.doKnightSpeak(id);
        }

        public void knightStory()
        {
            var sleepTime = Utils.getRandomMiliseconds(knightSprechenRange.Item1, knightSprechenRange.Item2);
            Utils.logEvent($"{name} opowiada przez {Utils.printTime(sleepTime)}s:  {Utils.getRandomStory()}");
            Thread.Sleep(sleepTime);
        }
    }
}