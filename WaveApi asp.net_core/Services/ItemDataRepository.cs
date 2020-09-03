using System;
using System.IO;
using WaveApi.Interfaces;
using WaveApi.Models;
using System.Drawing;
using WaveApi.Services;
using System.Collections.Generic;
using System.Linq;

namespace WaveApi.Services
{
    public class ItemDataRepository : IItemDataRepository
    {        
        
        ItemData itemData = new ItemData { };

        public ItemDataRepository()
        {
            InitializeData();
        }

        public ItemData All(string id)
        {
            try
            {
                string audioFilePath = Paths.audiosPath + "\\" + id + ".wav";
                string waveFilePath = Paths.wavesPath + "\\" + id + ".jpg";

                bool plikAudioIstnieje = File.Exists(audioFilePath);
                bool plikWaveIstnieje = File.Exists(waveFilePath);

                //wczytaj pliki audio i wave z dysku
                if (id != null)
                {
                    if (plikAudioIstnieje)
                    {
                        itemData.AudioFile = File.ReadAllBytes(audioFilePath);
                    }
                    else
                    {
                        itemData.AudioFile = null;
                    }

                    if (plikWaveIstnieje)
                    {
                        itemData.WavePicture = File.ReadAllBytes(waveFilePath);
                    }                    
                }
            }

            catch
            {
            }

            return itemData;
        }

        public void Insert(ItemData data)
        {       
            string audioFilePath = Paths.audiosPath + "\\" + data.ID + ".wav";

            //tworzy i zapisuje obraz fali
            ImageFilesProcessing.UtwórzObrazFali(data);

            //zapisuje plik audio na dysku
            if (data.AudioFile != null)
            {
                File.WriteAllBytes(audioFilePath, data.AudioFile);
                
                if (data.Active)
                {
                    //dodaj numer obrazu fali do pliku hashset
                    File.AppendAllText(Paths.HashSetPath, data.ID + Environment.NewLine);

                    //dodaj pozycję do zestawu obrazków do porównywania
                    var wavePicture = new WavePicture
                    {
                        ID = data.ID,
                        content = (Bitmap)new ImageConverter().ConvertFrom(data.WavePicture)
                    };
                    WavePictures.WavePicturesSet.Add(wavePicture);
                }
            }
        }

        public void Update(ItemData data)
        {
            string audioFilePath = Paths.audiosPath + "\\" + data.ID + ".wav";

            //zapisuje plik audio na dysku
            if (data.AudioFile != null)
            {
                File.WriteAllBytes(audioFilePath, data.AudioFile);
                
                AudioIDs.wczytajIDsSet();

                if (data.Active && !AudioIDs.AudioIDsSet.Contains(data.ID))
                {
                    //dodaj numer obrazu fali do pliku hashset
                    File.AppendAllText(Paths.HashSetPath, data.ID + Environment.NewLine);

                    //dodaj pozycję do zestawu obrazków do porównywania
                    var wavePicture = new WavePicture
                    {
                        ID = data.ID,
                        content = (Bitmap)new ImageConverter().ConvertFrom(data.WavePicture)
                    };
                    WavePictures.WavePicturesSet.Add(wavePicture);
                }

                if (!data.Active && AudioIDs.AudioIDsSet.Contains(data.ID))
                {
                    //usuń numer obrazu fali z pliku hashset
                    AudioIDs.AudioIDsSet.Remove(data.ID);

                    //zapisz hashset do pliku
                    File.WriteAllLines(Paths.HashSetPath, AudioIDs.AudioIDsSet);

                    //usuń pozycję z zestawu obrazków do porównywania
                    var wavePicture = WavePictures.WavePicturesSet.FirstOrDefault(_wavePicture => _wavePicture.ID == data.ID);
                    WavePictures.WavePicturesSet.Remove(wavePicture);
                }
            }
        }

        public void Delete(string id)
        {
            string audioFilePath = Paths.audiosPath + "\\" + id + ".wav";
            string waveFilePath = Paths.wavesPath + "\\" + id + ".jpg";

            File.Delete(audioFilePath);
            File.Delete(waveFilePath);

            AudioIDs.wczytajIDsSet();

            if (AudioIDs.AudioIDsSet.Contains(id))
            {
                //usuń numer obrazu fali z pliku hashset
                AudioIDs.AudioIDsSet.Remove(id);

                //zapisz hashset do pliku
                File.WriteAllLines(Paths.HashSetPath, AudioIDs.AudioIDsSet);

                //usuń pozycję z zestawu obrazków do porównywania
                var wavePicture = WavePictures.WavePicturesSet.FirstOrDefault(_wavePicture => _wavePicture.ID == id);
                WavePictures.WavePicturesSet.Remove(wavePicture);
            }
        }

        private void InitializeData()
        {

        }
    }
}