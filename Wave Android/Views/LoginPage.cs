using System;
using Xamarin.Forms;
using Wave;


namespace LoginPattern
{
	public class LoginPage : ContentPage
	{
		Entry username, password;

        ActivityIndicator activityIndicator = new ActivityIndicator { };

        Credentials credentials = new Credentials();
        private bool credentials_confirmed;

        public LoginPage (ILoginManager ilm)
		{
            var loginButton = new Button { Text = StartPage.lp.LogIn, FontSize = 24 };

            loginButton.Clicked += (sender, e) => 
            {
                activityIndicator.IsRunning = true;

                if (String.IsNullOrEmpty(username.Text) || String.IsNullOrEmpty(password.Text))
				{
					DisplayAlert(StartPage.lp.ValidationError, StartPage.lp.UsernamePasswordRequired, StartPage.lp.TryAgain);

                    activityIndicator.IsRunning = false;
                }
                else
                {
                    accountCheck();
                }
			};

            var createButton = new Button { Text = StartPage.lp.CreateAccount, FontSize = 24 };

            createButton.Clicked += (sender, e) => {
				MessagingCenter.Send<ContentPage> (this, "Create");
			};

			username = new Entry { Text = "" };
			password = new Entry { Text = "" };

			Content = new StackLayout {
				Padding = new Thickness (10, 40, 10, 10),
				Children = {
					new Label { Text = StartPage.lp.EnterData, FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) }, 
					new Label { Text = StartPage.lp.Username },
					username,
					new Label { Text = StartPage.lp.Password },
					password,
                    loginButton, createButton, activityIndicator
                }
			};
		}

        protected override void OnAppearing()
        {
            activityIndicator.IsRunning = false;
        }

        async void accountCheck()
        {
            credentials.username = username.Text;
            credentials.password = password.Text;
            
            await App.loginManager.SaveTaskAsync(credentials, false);
            
            //sprawdza, czy jest odpowiedź z serwera
            if (RestServiceLogin.response == null)
            {
                await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                var closer = DependencyService.Get<ICloseApplication>();
                closer.closeApplication();
            }

            if (RestServiceLogin.response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await DisplayAlert(StartPage.lp.UserNotFound, StartPage.lp.TryAgain, StartPage.lp.OK);
            }
            else
            {
                //pobiera potwierdzenie zgodności danych uwierzytelniających z serwera
                credentials_confirmed = await App.loginManager.GetTasksAsync();
                
                //sprawdza, czy jest odpowiedź z serwera
                if (RestServiceLogin.response == null)
                {
                    await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                    var closer = DependencyService.Get<ICloseApplication>();
                    closer.closeApplication();
                    //App.Current.Quit();
                }

                //znajdź na liście loginów i haseł wprowadzony przez użytkownika
                //Credentials _credentials = this.Find(credentials.username);

                if (credentials_confirmed == true)
                {
                    // REMEMBER LOGIN STATUS!
                    App.Current.Properties["IsLoggedIn"] = true;
                    App.Current.Properties["Username"] = credentials.username;
                    App.Current.Properties["Password"] = credentials.password;

                    await Navigation.PopAsync(true);

                    //var audioListPage = new AudioListPage();
                    //await Navigation.PushAsync(audioListPage, true);
                }
                else
                {
                    await DisplayAlert(StartPage.lp.WrongPassword, StartPage.lp.TryAgain, StartPage.lp.OK);

                    activityIndicator.IsRunning = false;
                }
            }
        }

        /*
        public Credentials Find(string username)
        {
            return credentialList.FirstOrDefault(credentials => credentials.username == username);
        }
        */
    }
}

