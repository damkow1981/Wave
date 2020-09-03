using System;
using Xamarin.Forms;
using AudioListREST;

namespace Wave
{
    public partial class AudioListPage : ContentPage
	{
        public AudioListPage()
		{
			InitializeComponent ();

            Title = StartPage.lp.AudioList;
        }

		protected async override void OnAppearing ()
		{
			base.OnAppearing ();

            //wczytanie menu
            listView.ItemsSource = await App.audioItemsManager.GetTasksAsync((string)App.Current.Properties["Username"]);
        }

        void OnAddItemClicked(object sender, EventArgs e)
        {
            var audioItem = new AudioItem();
            audioItem.Username = (string)App.Current.Properties["Username"];
            var audioPage = new AudioItemPage(true);
            audioPage.BindingContext = audioItem;
            Navigation.PushAsync(audioPage, true);
        }

        void OnItemSelected (object sender, SelectedItemChangedEventArgs e)
		{            
            var audioItem = e.SelectedItem as AudioItem;            
            var audioPage = new AudioItemPage();
			audioPage.BindingContext = audioItem;
			Navigation.PushAsync (audioPage, true);
		}

    }
}
