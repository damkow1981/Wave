using System.IO;
using System.Collections.Generic;


namespace WaveApi.Services
{
    public static class AudioIDs
    {                
        public static HashSet<string> AudioIDsSet;


        public static void wczytajIDsSet()
        {
            //odczytaj numery obrazów fali z pliku
            ImageFilesProcessing.CzekajNaPlik(Paths.HashSetPath);
            string[] FileLines = File.ReadAllLines(Paths.HashSetPath);

            //wczytaj numery obrazów fali do Hash zestawu
            AudioIDsSet = new HashSet<string>(FileLines);

            //usuń puste pozycje
            AudioIDsSet.Remove("");
        }
    }
}