using System;
using System.IO;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PhotoREST;
using LoginPattern;

namespace Wave
{
    public partial class MainPage : ContentPage
    {
        bool isNewItem;

        public static bool pictureTaken = false;

        bool isLoggedIn;

        public MainPage(bool isNew = false)
        {
            InitializeComponent();
            isNewItem = isNew;

            pictureTaken = false;

            Title = StartPage.lp.Title;
            ScanWave.Text = StartPage.lp.ScanWave;
            ManageAudios.Text = StartPage.lp.ManageAudios;
            
            ScanWave.Clicked += async (sender, args) =>
            {
                activityIndicator.IsRunning = true;

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert(StartPage.lp.NoCamera, StartPage.lp.CameraNotAvailable, StartPage.lp.OK);
                    return;
                }

                await DisplayAlert(StartPage.lp.MakingPhotoHints, StartPage.lp.Hints, StartPage.lp.OK);

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions      //Plugin.Media.Abstractions
                {
                    //Directory = "Scanned",
                    SaveToAlbum = false,
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Front,
                });

                activityIndicator.IsRunning = true;

                if (file == null)
                {
                    activityIndicator.IsRunning = false;
                    return;
                }
                pictureTaken = true;

                Photo photo = new Photo();
                photo.Picture = StreamToByte(file.GetStream());

                await App.photoManager.SaveTaskAsync(photo, isNewItem);

                //sprawdza, czy jest odpowiedź z serwera
                if (RestServicePhoto.response == null)
                {
                    await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                    var closer = DependencyService.Get<ICloseApplication>();
                    closer.closeApplication();
                };

                //pobiera obraz z serwera
                photo = await App.photoManager.GetTasksAsync();

                //sprawdza, czy jest odpowiedź z serwera

                if (RestServicePhoto.response == null)
                {
                    await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                    var closer = DependencyService.Get<ICloseApplication>();
                    closer.closeApplication();
                }

                /*
                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
                */

                //przejdź do strony wyświetlającej zdjęcie
                var viewPage = new ViewPagePhoto();
                viewPage.BindingContext = photo;
                
                await Navigation.PushAsync(viewPage, true);


                byte[] StreamToByte(Stream stream)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            activityIndicator.IsRunning = false;

            //App.Current.MainPage = new MainPage();

            isLoggedIn = App.Current.Properties.ContainsKey("IsLoggedIn") ? (bool)App.Current.Properties["IsLoggedIn"] : false;

            if (isLoggedIn)
            {
                LoggingButton.Text = StartPage.lp.LogOut;
                ManageAudios.IsEnabled = true;
            }
            else
            {
                LoggingButton.Text = StartPage.lp.LogIn;
                ManageAudios.IsEnabled = false;
            }
        }

        async void ManageAudios_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;

            Credentials credentials = new Credentials();
            credentials.username = App.Current.Properties.ContainsKey("Username") ? (string)App.Current.Properties["Username"] : "";
            credentials.password = App.Current.Properties.ContainsKey("Password") ? (string)App.Current.Properties["Password"] : "";

            await App.loginManager.SaveTaskAsync(credentials, false);

            //sprawdza, czy jest odpowiedź z serwera
            if (RestServiceLogin.response == null)
            {
                await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                var closer = DependencyService.Get<ICloseApplication>();
                closer.closeApplication();
            }

            var itemListPage = new AudioListPage();
            await Navigation.PushAsync(itemListPage, true);
        }
        
        async void Logging_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;

            if (isLoggedIn)
            {
                App.Current.Properties["IsLoggedIn"] = false;
                isLoggedIn = false;
                LoggingButton.Text = StartPage.lp.LogIn;
                ManageAudios.IsEnabled = false;

                await DisplayAlert(StartPage.lp.LoggedOut, StartPage.lp.YouAreLoggedOut, StartPage.lp.OK);

                activityIndicator.IsRunning = false;
            }
            else
            {
                var loginModalPage = new LoginModalPage(App.Current);
                await Navigation.PushAsync(loginModalPage);
            }
        }
    }
}
