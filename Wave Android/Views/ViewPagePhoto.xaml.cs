using System;
using System.IO;
using Xamarin.Forms;
using Plugin.AudioRecorder;
using PhotoREST;

namespace Wave
{
    public partial class ViewPagePhoto : ContentPage
	{        
        AudioPlayer player;

        public ViewPagePhoto ()
		{
			InitializeComponent ();

            Title = StartPage.lp.WavePreview;

            player = new AudioPlayer();
        }

		protected async override void OnAppearing ()
		{
			base.OnAppearing ();

            var photo = (Photo)BindingContext;

            if (MainPage.pictureTaken)
            {
                //wyświetla obraz
                image.Source = ImageSource.FromStream(() => new MemoryStream(photo.Picture));

                //odtwarza i zapisuje plik audio na urządzeniu
                if (photo.AudioFile != null)
                {
                    string audioFilePath = null;

                    //zapisz plik audio w telefonie aby móc go odtworzyć
                    try
                    {
                        //string audioFilePath = "/storage/emulated/0/Pictures/" + itemData.Name + ".wav";
                        //File.WriteAllBytes(audioFilePath, itemData.AudioFile);

                        audioFilePath = DependencyService.Get<IAudio>().SaveAudioToDisk("Audio_found", photo.AudioFile);

                        //pokaż użytkownikowi ścieżkę do pliku
                        //await DisplayAlert(StartPage.lp.AudioFileLocation, audioFilePath, StartPage.lp.OK);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    //odtwarza plik audio
                    try
                    {
                        player.Play(audioFilePath);
                    }
                    catch (Exception ex)
                    {
                        //blow up the app!
                        throw ex;
                    }

                    /*
                    //zapisuje obraz
                    if (photo.Picture != null)
                    {
                        string pictureFilePath = DependencyService.Get<IPicture>().SavePictureToDisk("WAVE", photo.Picture);

                        //pokaż użytkownikowi ścieżkę do pliku
                        await DisplayAlert("Picture file Location", pictureFilePath, "OK");
                    }
                    */
                }
                else
                {
                    await DisplayAlert(StartPage.lp.AudioNotFound, StartPage.lp.TryToMakePictureMoreAccurate, StartPage.lp.OK);
                }
            }
        }
        
        protected override void OnDisappearing()
        {
            //usuwa obraz z ekranu
            image.Source = ImageSource.FromStream(() => new MemoryStream());

            //var startPage = new StartPage();
            //Navigation.PushAsync(startPage, false);
        }
    }
}
