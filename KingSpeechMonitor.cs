namespace monitorsProj
{
    //symulacja monitora do mówienia króla
    public class KingSpeechMonitor
    {
        //obiekt na którym zakładany jest lock
        private readonly object kingSpeakingLock = new object();
        //zapalane i gaszone wyłącznie przez króla, sprawdzane przez rycerzy:
        private bool isKingSpeaking = false;
        //w tej kolejce czekają rycerze:
        private ConditionVariable kingNotSpeaking = new ConditionVariable();

        public KingSpeechMonitor ()
        {

        }

        //ENTRIES

        //po zakończeniu mówienia król zwalnia rycerzy z rozkazu zamknięcia się
        public void ReleaseKnightQueue ()
        {
            lock (kingSpeakingLock)
            {
                isKingSpeaking = false;
                kingNotSpeaking.Pulse();
                return;
            }
        }

        public void ShutUp ()
        {
            lock (kingSpeakingLock)
            {
                isKingSpeaking = true;
                Utils.logEvent("*KRÓL* mówi: ZAMKNĄĆ SIĘ! TERAZ JA MÓWIĘ");
            }
        }

        //jeżeli król mówi, poczekaj na koniec (potem zwalnianie sekwencyjne)
        public void WaitIfKingIsSpeaking (string name = "")
        {
            lock (kingSpeakingLock)
            {
                if (isKingSpeaking)
                {
                    Utils.logEvent($"{name} czeka aż król skończy mówić.");
                    while (isKingSpeaking) //spurious wakeup
                        kingNotSpeaking.Wait(kingSpeakingLock);
                    Utils.logEvent($"{name} może teraz mówić.");
                    //zwalnianie kolejnego rycerza (by nie używać PulseAll)
                    kingNotSpeaking.Pulse();
                }
            }
        }


    }
}