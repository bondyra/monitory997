namespace monitory997
{
    //symulacja monitora do mówienia króla
    public class KingSpeechMonitor
    {
        //obiekt na którym zakładany jest lock
        private readonly object kingSpeakingLock = new object();
        //zapalane i gaszone wyłącznie przez króla, sprawdzane przez rycerzy:
        private bool isKingSpeaking = false;
        //na tej kolejce czekają rycerze:
        private ConditionVariable kingNotSpeaking = new ConditionVariable();

        public KingSpeechMonitor ()
        {

        }

        //ENTRIES

        //po zakończeniu mówienia król zwalnia rycerzy z rozkazu zamknięcia mordy
        public void ReleaseKnightQueue ()
        {
            lock (kingSpeakingLock)
            {
                isKingSpeaking = false;
                kingNotSpeaking.PulseAll();
            }
        }

        public void ShutTheFuckUp ()
        {
            lock (kingSpeakingLock)
            {
                isKingSpeaking = true;
                Utils.logEvent("*KRÓL* mówi: ZAMKNĄĆ MORDY JEBANE SKURWYSYNY!");
            }
        }

        //jeżeli król mówi, poczekaj na koniec (aż puści PulseAll)
        public void WaitIfKingIsSpeaking (string name = "")
        {
            lock (kingSpeakingLock)
            {
                if (isKingSpeaking)
                {
                    Utils.logEvent($"{name} czeka aż król skończy mówić.");
                    while (isKingSpeaking) //spurious wakeup (tak było na slajdach)
                        kingNotSpeaking.Wait(kingSpeakingLock);
                    Utils.logEvent($"{name} może teraz mówić.");
                }
            }
        }


    }
}