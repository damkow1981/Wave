using System.Collections.Generic;
using System.Drawing;
using WaveApi.Models;

namespace WaveApi.Services
{
    public static class WavePictures
    {
        public static HashSet<WavePicture> WavePicturesSet = new HashSet<WavePicture>();

        public static void wczytajObrazki()
        {
            AudioIDs.wczytajIDsSet();

            foreach (string numer in AudioIDs.AudioIDsSet)
            {
                var wavePicture = new WavePicture
                {
                    ID = numer,
                    content = (Bitmap)Image.FromFile(Paths.wavesPath + numer + ".jpg")
                };
                //wczytaj plik z obrazkiem fali z dysku
                WavePicturesSet.Add(wavePicture);
            }
        }
    }
}