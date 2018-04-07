using System.Linq;
namespace monitorsProj
{
    //monitor do kontroli nad zasobami
    public class ResourcesMonitor
    {
        //stałe:
        private readonly int peopleNumber;
        private readonly int cucumberCount;
        private readonly int wineVolume;

        //monitor
        private object monitorObject = new object();

        //chronione przez monitor:
        //wino
        private int wineBottle;
        //ogórki w tablicy
        private int[] cucumbers;

        public ResourcesMonitor(int _wineVolume, int _cucumberCount, int _peopleNumber)
        {
            System.Diagnostics.Debug.Assert(_peopleNumber%2 == 0);
            peopleNumber = _peopleNumber;
            wineVolume = _wineVolume;
            cucumberCount = _cucumberCount;
            cucumbers = new int[_peopleNumber/2];
            setUpResources();
        }
        
        //ustawianie początkowego stanu zasobów
        private void setUpResources(){
            wineBottle = wineVolume;
            for (int i=0;i<cucumbers.Length;i++)
                cucumbers[i] = cucumberCount;
        }

        //ENTRIES

        //slużący uzupełnia wino/ogórki
        public void ReplenshWineBottle(){
            lock (monitorObject){
                wineBottle = wineVolume;
                logResources();
            }
        }

        public void ReplenishCucumberPlates(){
            lock (monitorObject){
                for (int i=0;i<cucumbers.Length;i++)
                    cucumbers[i] = cucumberCount;
                logResources();
            }            
        }

        //rycerz z talerzem i kielichem bierze swoje zasoby
        //zwracane jest powodzenie operacji
        public bool TryEat(int id){
            var name = Utils.getName(id);
            bool hasDined = false;
            lock (monitorObject){
                int plateId = (id/2 + id%2) % (peopleNumber/2);
                bool isWine = wineBottle > 0;
                bool isCucumber = cucumbers[plateId] > 0;
                if (isCucumber && isWine){
                    hasDined = true;
                    Utils.logEvent($"{name} ZJADŁ!");
                    cucumbers[plateId]--;
                    wineBottle--;
                }
                else{
                    Utils.logEvent($"{name} NIE ZJADŁ, nie było: {getFancyLackString(isWine, isCucumber)}.");
                }
            }
                return hasDined;
        }

        private string getFancyLackString (bool isWine, bool isCucumber){
            System.Diagnostics.Debug.Assert(!isWine || !isCucumber);
            if (!isWine && !isCucumber)
                return "WSZYSTKIEGO";
            if (!isWine)
                return "WINA";
            return "OGÓRKA";
        }

        private void logResources(){
            Utils.logEvent("ZASOBY:");
            Utils.logEvent($"STAN KARAFKI: {wineBottle}.");
            Utils.logEvent($"OGÓRKI: [{string.Join(",",cucumbers)}]");
        }
    }
}