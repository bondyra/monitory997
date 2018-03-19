using System;

namespace monitory997
{
    public class Monitory
    {
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
