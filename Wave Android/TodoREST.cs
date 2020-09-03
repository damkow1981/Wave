using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LoginPattern;
using AudioListREST;
using PhotoREST;
using DataREST;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Wave
{
	public class App : Application, ILoginManager
    {
        public static AudioItemsManager audioItemsManager { get; private set; }
        public static DataManager dataManager { get; private set; }
        public static PhotoManager photoManager { get; private set; }
        public static LoginManager loginManager { get; private set; }

        public static ITextToSpeech Speech { get; set; }

        public new static App Current;

        public static bool isLoggedIn;
        
        public App ()
		{
            Current = this;

            audioItemsManager = new AudioItemsManager (new RestServiceAudio ());
            dataManager = new DataManager(new RestServiceData());
            loginManager = new LoginManager(new RestServiceLogin());
            photoManager = new PhotoManager(new RestServicePhoto());

            MainPage = new NavigationPage(new StartPage ());

            isLoggedIn = Properties.ContainsKey("IsLoggedIn") ? (bool)Properties["IsLoggedIn"] : false;
        }

        public void ShowMainPage()
        {
            //MainPage = new MainPage();
        }

        public void Logout()
        {
            Properties["IsLoggedIn"] = false; // only gets set to 'true' on the LoginPage
            MainPage = new LoginModalPage(this);
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

