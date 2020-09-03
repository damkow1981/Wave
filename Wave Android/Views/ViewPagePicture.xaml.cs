using System.IO;
using Xamarin.Forms;
using Plugin.AudioRecorder;
using DataREST;

namespace Wave
{
    public partial class ViewPagePicture : ContentPage
	{        
        AudioPlayer player = new AudioPlayer();

        public ViewPagePicture()
		{
			InitializeComponent ();

            Title = StartPage.lp.WavePreview;
        }

		protected async override void OnAppearing ()
		{
			base.OnAppearing ();

            var ItemData = (ItemData)BindingContext;

            //wyświetla obraz
            image.Source = ImageSource.FromStream(() => new MemoryStream(ItemData.WavePicture));
        }
        
        protected override void OnDisappearing()
        {
            //usuwa obraz z ekranu
            image.Source = ImageSource.FromStream(() => new MemoryStream());
        }
    }
}
