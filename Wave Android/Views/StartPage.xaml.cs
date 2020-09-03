using Xamarin.Forms;

namespace Wave
{
    public partial class StartPage : ContentPage
	{
        bool alertShown = false;

        public static string language;
        public static LanguagePack lp;

        public StartPage ()
		{
			InitializeComponent ();

            //wersja językowa
            language = "PL";
            lp = new LanguagePack();
        }

		protected async override void OnAppearing ()
		{
			base.OnAppearing ();

			if (Constants.RestUrl.Contains ("developer.xamarin.com")) {
				if (!alertShown) {
					await DisplayAlert (
						"Hosted Back-End",
						"This app is running against Xamarin's read-only REST service. To create, edit, and delete data you must update the service endpoint to point to your own hosted REST service.",
						"OK");
					alertShown = true;				
				}
			}

            App.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}
