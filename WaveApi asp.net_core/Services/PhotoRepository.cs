using System.IO;
using WaveApi.Interfaces;
using WaveApi.Models;
using System.Drawing;

namespace WaveApi.Services
{
    public class PhotoRepository : IPhotoRepository
    {
        private Photo _photo = new Photo();

        public PhotoRepository()
        {
            InitializeData();
        }

        public Photo Get
        {
            get
            {
                try
                {
                    bool plikIstnieje = File.Exists(Paths.audiosPath + _photo.FileName);

                    //jeśli plik audio nie istnieje, to go utwórz
                    if ((!plikIstnieje) && (_photo.FileName == null))
                    {
                        File.Create(Paths.audiosPath + _photo.FileName + ".wav").Dispose();
                    }

                    //wczytaj plik audio z dysku
                    if (_photo.FileName != null)
                    {
                        _photo.AudioFile = File.ReadAllBytes(Paths.audiosPath + _photo.FileName + ".wav");
                    }
                }


                catch
                {
                }

                return _photo;
            }
        }

        public void Post(Photo photo)
        {

        }

        public void Put(Photo photo)
        {
            //konwertuj byte do bitmap            
            Bitmap bitmap = (Bitmap)new ImageConverter().ConvertFrom(photo.Picture);

            //transformuj i zapisz zdjęcie na dysku
            bitmap = ImageFilesProcessing.ZapiszZdjęcie(bitmap);

            //aby telefon wyświetlał już obrobiony obrazek
            photo.Picture = ImageFilesProcessing.convertImageToByte(bitmap);

            //Bitmap picture;

            foreach (WavePicture picture in WavePictures.WavePicturesSet)
            {
                //wczytaj plik z obrazkiem fali z dysku
                //picture = (Bitmap)(Image.FromFile(Paths.wavesPath + numer + ".jpg"));
                
                if (ImageFilesProcessing.CompareImages(bitmap, picture.content))
                {
                    //numer znalezionego pliku
                    photo.FileName = picture.ID;

                    //wczytaj plik audio z dysku
                    ImageFilesProcessing.CzekajNaPlik(Paths.audiosPath + picture.ID + ".wav");
                    photo.AudioFile = File.ReadAllBytes(Paths.audiosPath + picture.ID + ".wav");

                    break;
                }
            }

            _photo = photo;
        }

        private void InitializeData()
        {
            //AudioIDs.wczytajIDsSet();
        }
    }
}