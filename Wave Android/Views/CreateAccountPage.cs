using System;
using Xamarin.Forms;
using Wave;

namespace LoginPattern
{
	public class CreateAccountPage : ContentPage
	{
        Entry username, password, confirmPassword;

        public CreateAccountPage (ILoginManager ilm)
		{
			var createButton = new Button { Text = StartPage.lp.CreateAccount, FontSize = 24 };

            createButton.Clicked += (sender, e) => 
            {
                if (String.IsNullOrEmpty(username.Text) || String.IsNullOrEmpty(password.Text) || String.IsNullOrEmpty(confirmPassword.Text))
                {
                    DisplayAlert(StartPage.lp.ValidationError, StartPage.lp.UsernamePasswordRequired, StartPage.lp.TryAgain);
                }
                else if (password.Text != confirmPassword.Text)
                {
                    DisplayAlert(StartPage.lp.ValidationError, StartPage.lp.PasswordsDoNotMatch, StartPage.lp.TryAgain);
                }
                else
                {
                    sendCredentials();

                    //var todoListPage = new TodoListPage();
                    //Navigation.PushAsync(todoListPage, true);
                }
                
            };

			var cancelButton = new Button { Text = StartPage.lp.Cancel, FontSize = 24 };

            cancelButton.Clicked += (sender, e) => {
				MessagingCenter.Send<ContentPage> (this, "Login");
			};
            
            username = new Entry { Text = "" };
            password = new Entry { Text = "" };
            confirmPassword = new Entry { Text = "" };

            Content = new StackLayout {
				Padding = new Thickness (10, 40, 10, 10),
				Children = {
					new Label { Text = StartPage.lp.EnterData, Font = Font.SystemFontOfSize(NamedSize.Large) }, 
					new Label { Text = StartPage.lp.ChooseUsername },
                    username,
					new Label { Text = StartPage.lp.Password },
                    password,
					new Label { Text = StartPage.lp.ReEnterPassword },
                    confirmPassword,
                    createButton, cancelButton
                }
			};
		}
        
        async void sendCredentials()
        {
            Credentials credentials = new Credentials()
            {
                username = username.Text,
                password = password.Text,
            };

            await App.loginManager.SaveTaskAsync(credentials, true);
            
            //sprawdza, czy jest odpowiedź z serwera
            if (RestServiceLogin.response == null)
            {
                await DisplayAlert(StartPage.lp.ServerUnavailable, StartPage.lp.TryAgainLater, StartPage.lp.OK);

                var closer = DependencyService.Get<ICloseApplication>();
                closer.closeApplication();
            }
            else if (RestServiceLogin.response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                await DisplayAlert(StartPage.lp.UserAlreadyExists, StartPage.lp.TryAgain, StartPage.lp.OK);
            }
            else
            {
                //await DisplayAlert(StartPage.lp.AccountCreated, StartPage.lp.NowYouCanLogIn, StartPage.lp.OK);
                await DisplayAlert(StartPage.lp.AccountCreated, StartPage.lp.YouAreLoggedIn, StartPage.lp.OK);

                //przekieruj do strony logowania
                //await Navigation.PushAsync(App.LoginPage, true);

                App.Current.Properties["IsLoggedIn"] = true;
                App.Current.Properties["Username"] = credentials.username;
                App.Current.Properties["Password"] = credentials.password;

                await Navigation.PopAsync(true);
            }
        }
    }
}

