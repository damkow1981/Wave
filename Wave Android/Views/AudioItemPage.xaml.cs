using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.AudioRecorder;
using DataREST;
using AudioListREST;
using System.Collections.Generic;
using System.Linq;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using System.Diagnostics;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace Wave
{
	public partial class AudioItemPage : ContentPage
	{
        bool isNewItem;

        AudioRecorderService recorder;
        AudioPlayer player;

        List<AudioItem> audioList;
        ItemData itemData;
        string ID;

        bool paidAlready;

        public static bool audioRecorded;        
        string _audioFilePath;

        string previousName;

        public AudioItemPage (bool isNew = false)  
        {
			InitializeComponent ();
			isNewItem = isNew;
            paidAlready = false;

            nameEntry.Placeholder = StartPage.lp.Placeholder;
            IsActive.Text = StartPage.lp.IsActive;
            StopRecordingAfterSilence.Text = StartPage.lp.StopRecordingAfterSilence;
            RecordButton.Text = StartPage.lp.Record;
            SaveButton.Text = StartPage.lp.Save;
            PlayButton.Text = StartPage.lp.Play;
            ViewButton.Text = StartPage.lp.View;
            DeleteButton.Text = StartPage.lp.Delete;

            audioRecorded = false;

            recorder = new AudioRecorderService
            {
                StopRecordingAfterTimeout = true,
                TotalAudioTimeout = TimeSpan.FromSeconds(15),
                AudioSilenceTimeout = TimeSpan.FromSeconds(2)
            };
            
            player = new AudioPlayer();
            player.FinishedPlaying += Player_FinishedPlaying;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();            

            var AudioItem = (AudioItem)BindingContext;

            previousName = AudioItem.Name;

            if (AudioItem.ID != null)
            {
                itemData = await App.dataManager.GetTasksAsync(AudioItem.ID);
            }
            else
            {
                itemData = await App.dataManager.GetTasksAsync(ID);
            }

            //sprawdza, czy jest odpowiedź z serwera
            if (RestServiceData.response == null)
            {
                await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                var closer = DependencyService.Get<ICloseApplication>();
                closer.closeApplication();
            }

            if (itemData.WavePicture != null) ViewButton.IsEnabled = true;

            //zapisz plik audio w telefonie aby móc go odtworzyć
            try
            {
                if (itemData.AudioFile != null)
                {
                    //string _audioFilePath = "/storage/emulated/0/Music/" + AudioItem.Name + ".wav";
                    // lub
                    //string _audioFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic), AudioItem.Name + ".wav");
                    
                    //File.WriteAllBytes(_audioFilePath, itemData.AudioFile);

                    _audioFilePath = DependencyService.Get<IAudio>().SaveAudioToDisk(AudioItem.Name, itemData.AudioFile);
                    PlayButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            var status = PermissionStatus.Unknown;
            status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);

            //await DisplayAlert("Pre - Results", status.ToString(), "OK");

            if (status != PermissionStatus.Granted) status = await Utils.CheckPermissions(Permission.Microphone);

            //await DisplayAlert("Results", status.ToString(), "OK");

            RecordButton.IsEnabled = true;
            SaveButton.IsEnabled = true;
            DeleteButton.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }

        async void Record_Clicked(object sender, EventArgs e)
        {
            await RecordAudio();
        }

        async Task RecordAudio()
        {
            try
            {
                if (!recorder.IsRecording) //Record button clicked
                {
                    recorder.StopRecordingOnSilence = TimeoutSwitch.IsToggled;

                    RecordButton.IsEnabled = false;
                    PlayButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;

                    //start recording audio
                    var audioRecordTask = await recorder.StartRecording();

                    RecordButton.Text = StartPage.lp.StopRecording;
                    RecordButton.IsEnabled = true;

                    await audioRecordTask;

                    RecordButton.Text = StartPage.lp.Record;
                    SaveButton.IsEnabled = true;
                }
                else //Stop button clicked
                {
                    RecordButton.IsEnabled = false;

                    //stop the recording...
                    await recorder.StopRecording();

                    RecordButton.IsEnabled = true;
                }

                //wczytuje plik audio
                if (recorder.GetAudioFilePath() != null)
                {
                    string filePath = recorder.GetAudioFilePath();
                    itemData.AudioFile = File.ReadAllBytes(filePath);
                    
                    PlayButton.IsEnabled = true;
                    audioRecorded = true;

                    paidAlready = false;
                }
                else audioRecorded = false;
            }
            catch (Exception ex)
            {
                //blow up the app!
                throw ex;
            }
        }

        void Play_Clicked(object sender, EventArgs e)
        {
            string audioFilePath;

            if (audioRecorded)
            {
                audioFilePath = recorder.GetAudioFilePath();
            }

            else
            {
                audioFilePath = _audioFilePath;
            }

            PlayAudio(audioFilePath);
        }

        void PlayAudio(string filePath)
        {
            try
            {
                if (filePath != null)
                {
                    PlayButton.IsEnabled = false;
                    RecordButton.IsEnabled = false;

                    player.Play(filePath);
                }
            }
            catch (Exception ex)
            {
                //blow up the app!
                //throw ex;
            }
        }

        void Player_FinishedPlaying(object sender, EventArgs e)
        {
            PlayButton.IsEnabled = true;
            RecordButton.IsEnabled = true;            
        }

        async void View_Clicked(object sender, EventArgs e)
        {
            //przejdź do strony wyświetlającej zdjęcie
            var viewPage = new ViewPagePicture();
            viewPage.BindingContext = itemData;
            await Navigation.PushAsync(viewPage, true);
        }
        
        async void OnSaveActivated (object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;

            await synchro();

            Task synchro()
            {            
                var AudioItem = (AudioItem)BindingContext;

                if (AudioItem.Name == null || AudioItem.Name == "")
                {
                    DisplayAlert(StartPage.lp.NameRequired, StartPage.lp.PleaseEnterName, StartPage.lp.OK);

                    activityIndicator.IsRunning = false;

                    return Task.Delay(0);
                }

                //pobiera pieniądze tylko za aktywne obrazki i tylko gdy jest nagrana ścieżka audio
                if (itemData.AudioFile != null && AudioItem.Active && !paidAlready)
                {
                    double price = 0.10;

                    paypal();

                    async void paypal()
                    {
                        var result = await CrossPayPalManager.Current.Buy(new PayPalItem(AudioItem.Name, new Decimal(price), "PLN"), new Decimal(0.0));
                        if (result.Status == PayPalStatus.Cancelled)
                        {
                            activeSwitch.IsToggled = false;

                            await DisplayAlert(StartPage.lp.Cancelled, "", StartPage.lp.OK);
                              
                            Save();

                            activityIndicator.IsRunning = false;

                            return;
                        }
                        else if (result.Status == PayPalStatus.Error)
                        {
                            activeSwitch.IsToggled = false;

                            await DisplayAlert(StartPage.lp.ValidationError, result.ErrorMessage, StartPage.lp.OK);

                            Save();

                            activityIndicator.IsRunning = false;

                            return;
                        }
                        else if (result.Status == PayPalStatus.Successful)
                        {     
                            await DisplayAlert(StartPage.lp.PaymentAccepted, "", StartPage.lp.OK);

                            Debug.WriteLine(result.ServerResponse.Response.Id);

                            paidAlready = true;

                            Save();

                            activityIndicator.IsRunning = false;

                            return;
                        }
                    }
                }

                //DisplayAlert(StartPage.lp.ItemSaved, StartPage.lp.WaveAvailableForScanning, StartPage.lp.OK);

                Save();

                return Task.Delay(0);
            }            
        }

        async void OnDeleteActivated (object sender, EventArgs e)
		{
            //upewnij się, że użytkownik chce usunąć obraz
            bool confirmed = await DisplayAlert(StartPage.lp.Confirm, StartPage.lp.DoYouReallyWantToRemove, StartPage.lp.OK, StartPage.lp.Cancel);

            if (confirmed)
            {
                var audioItem = (AudioItem)BindingContext;
                await App.audioItemsManager.DeleteTaskAsync(audioItem);
                
                //sprawdza, czy jest odpowiedź z serwera
                if (RestServiceAudio.response == null)
                {
                    await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                    var closer = DependencyService.Get<ICloseApplication>();
                    closer.closeApplication();
                };

                await App.dataManager.DeleteTaskAsync(audioItem.ID);


                //sprawdza, czy jest odpowiedź z serwera
                if (RestServiceData.response == null)
                {
                    await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                    var closer = DependencyService.Get<ICloseApplication>();
                    closer.closeApplication();
                };

                await Navigation.PopAsync();
            }
		}

        async void Save()
        {
            var AudioItem = (AudioItem)BindingContext;

            if (previousName != AudioItem.Name)
            {
                isNewItem = true;
                previousName = AudioItem.Name;
            }
            else isNewItem = false;

            await App.audioItemsManager.SaveTaskAsync(AudioItem, isNewItem);

            //sprawdza, czy jest odpowiedź z serwera
            if (RestServiceAudio.response == null)
            {
                await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                var closer = DependencyService.Get<ICloseApplication>();
                closer.closeApplication();
            }

            //sprawdza, czy element jest już na liście
            if (RestServiceAudio.response.StatusCode == HttpStatusCode.Conflict)
            {
                await DisplayAlert(StartPage.lp.ItemAlreadyExists, StartPage.lp.TryGivingDifferentName, StartPage.lp.OK);

                activityIndicator.IsRunning = false;                

                return;
            }

            audioList = await App.audioItemsManager.GetTasksAsync(AudioItem.Username);

            //sprawdza, czy jest odpowiedź z serwera
            if (RestServiceAudio.response == null)
            {
                await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                var closer = DependencyService.Get<ICloseApplication>();
                closer.closeApplication();
            }

            AudioItem = audioList.FirstOrDefault(item => item.Name == AudioItem.Name);

            ID = AudioItem.ID;

            itemData.ID = AudioItem.ID;
            itemData.Active = AudioItem.Active;

            await App.dataManager.SaveTaskAsync(itemData, isNewItem);

            //sprawdza, czy jest odpowiedź z serwera
            if (RestServiceData.response == null)
            {
                await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                var closer = DependencyService.Get<ICloseApplication>();
                closer.closeApplication();
            }
            
            itemData = await App.dataManager.GetTasksAsync(AudioItem.ID);

            //sprawdza, czy jest odpowiedź z serwera
            if (RestServiceData.response == null)
            {
                await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                var closer = DependencyService.Get<ICloseApplication>();
                closer.closeApplication();
            }

            ViewButton.IsEnabled = true;

            //zapisuje obraz w telefonie
            if (isNewItem && itemData.WavePicture != null)
            {
                string filePath = DependencyService.Get<IPicture>().SavePictureToDisk(AudioItem.Name, itemData.WavePicture);

                //pokaż użytkownikowi ścieżkę do pliku
                await DisplayAlert(StartPage.lp.PictureFileLocation, filePath, StartPage.lp.OK);
            }

            if (audioRecorded)
            {
                SaveAudio();
            }
            else
            {
                activityIndicator.IsRunning = false;
            }

            //await Navigation.PopAsync ();     //powrót do strony głównej

            return;
        }

        async void SaveAudio()
        {
            //zapisz plik audio w telefonie
            try
            {
                //string audioFilePath = "/storage/emulated/0/Pictures/" + itemData.Name + ".wav";
                //File.WriteAllBytes(audioFilePath, itemData.AudioFile);

                var AudioItem = (AudioItem)BindingContext;

                _audioFilePath = DependencyService.Get<IAudio>().SaveAudioToDisk(AudioItem.Name, itemData.AudioFile);

                //pokaż użytkownikowi ścieżkę do pliku
                await DisplayAlert(StartPage.lp.AudioFileLocation, _audioFilePath, StartPage.lp.OK);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            activityIndicator.IsRunning = false;
        }
        
        public AudioItem Find(string Name)
        {
            return audioList.FirstOrDefault(item => item.Name == Name);
        }

        /*

		void OnCancelActivated (object sender, EventArgs e)
		{
			Navigation.PopAsync ();
		}

		void OnSpeakActivated (object sender, EventArgs e)
		{
			var todoItem = (TodoItem)BindingContext;
			App.Speech.Speak (todoItem.Name + " " + todoItem.AudioFile);
		}

        */
    }
}
