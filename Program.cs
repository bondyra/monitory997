using System;

namespace monitory997
{
    public class Monitory
    {
        private Random r = new Random(997);

        private int getRandomMiliseconds(double min, double max){
            double m = r.NextDouble();
            return (int)(1000*(min+m*(max-min)));
        }

        private void logEvent(String eventText)
        {
            System.Console.WriteLine(System.DateTime.Now + " " + eventText);
        }

        public void run ()
        {   
            logEvent("Hello Monitory997!");
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
