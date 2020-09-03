namespace Wave
{
	public class LanguagePack
    {
        //Main page
        public string Title;
        public string ScanWave;
        public string NoCamera;
        public string CameraNotAvailable;
        public string OK;
        public string MakingPhotoHints;
        public string Hints;
        public string ServerUnavailable;
        public string TryAgainLater;
        public string LogIn;
        public string LogOut;
        public string LoggedOut;
        public string YouAreLoggedIn;
        public string YouAreLoggedOut;
        public string ManageAudios;

        //View page
        public string WavePreview;
        public string AudioNotFound;
        public string TryToMakePictureMoreAccurate;

        //Login page
        public string CreateAccount;
        public string EnterData;
        public string Username;
        public string Password;
        public string ValidationError;
        public string UsernamePasswordRequired;        
        public string UserNotFound;
        public string TryAgain;
        public string WrongPassword;

        //Create account page
        public string PasswordsDoNotMatch;
        public string Cancel;
        public string ChooseUsername;
        public string ReEnterPassword;
        public string UserAlreadyExists;
        public string AccountCreated;
        public string NowYouCanLogIn;

        //Audio list
        public string AudioList;

        //Audio item
        public string Placeholder;
        public string IsActive;
        public string StopRecordingAfterSilence;
        public string Record;
        public string Save;
        public string Play;
        public string View;
        public string Delete;
        public string StopRecording;
        public string NameRequired;
        public string PleaseEnterName;
        public string Cancelled;
        public string PaymentAccepted;
        public string ItemAlreadyExists;
        public string TryGivingDifferentName;
        public string PictureFileLocation;
        public string AudioFileLocation;
        public string Confirm;
        public string DoYouReallyWantToRemove;
        public string ItemSaved;
        public string WaveAvailableForScanning;
        
        public LanguagePack()
        {   
            switch (StartPage.language)
            {
                case "EN":

                    //Main page
                    Title = "Wave";
                    ScanWave = "Scan wave";
                    NoCamera = "No camera";
                    CameraNotAvailable = ":( No camera available.";
                    OK = "OK";
                    MakingPhotoHints = "Making photo hints";
                    Hints = "Orient Your smartphone horizontally. " +
                        "Whole picture should be visible on screen and it should fill the screen as much as possible. " +
                        "Background should be possibly bright and uniform.";
                    ServerUnavailable = "Server unavailable";
                    TryAgainLater = "Try again later";
                    LogIn = "Log in";
                    LogOut = "Log out";
                    LoggedOut = "Logged out";
                    YouAreLoggedIn = "You are now logged into Your account";
                    YouAreLoggedOut = "You are now logged out from Your account";
                    ManageAudios = "Manage Audios";

                    //View page
                    WavePreview = "Wave preview";
                    AudioNotFound = "Audio not found";
                    TryToMakePictureMoreAccurate = "Try to make the picture more accurate";

                    //Login page
                    CreateAccount = "Create Account";
                    EnterData = "Enter data";
                    Username = "Username";
                    Password = "Password";
                    ValidationError = "Validation Error";
                    UsernamePasswordRequired = "Username and Password are required";
                    UserNotFound = "User not found";
                    TryAgain = "Try again";
                    WrongPassword = "Wrong password";

                    //Create account page
                    PasswordsDoNotMatch = "Password does not match confirmed password";
                    Cancel = "Cancel";
                    ChooseUsername = "Choose a Username";
                    ReEnterPassword = "Re-enter Password";
                    UserAlreadyExists = "User already exists";
                    AccountCreated = "Account created";
                    NowYouCanLogIn = "Now You can log into Your account";

                    //Audio list
                    AudioList = "Audio List";

                    //Audio item
                    Placeholder = "wave name";
                    IsActive = "Is active";
                    StopRecordingAfterSilence = "Stop recording after silence?";
                    Record = "Record sound";
                    Save = "Save";
                    Play = "Play sound";
                    View = "View picture";
                    Delete = "Delete";
                    StopRecording = "Stop Recording";
                    NameRequired = "Name required";
                    PleaseEnterName = "Please enter the name of the wave";
                    Cancelled = "Cancelled";
                    PaymentAccepted = "Payment accepted";
                    ItemAlreadyExists = "Item already exists";
                    TryGivingDifferentName = "Try giving different name";
                    PictureFileLocation = "Picture file location";
                    AudioFileLocation = "Audio file location";
                    Confirm = "Confirm";
                    DoYouReallyWantToRemove = "Do You really want to remove this item?";
                    ItemSaved = "Item saved";
                    WaveAvailableForScanning = "Wave picture is available for scanning now";

                    break;

                case "PL":

                    //Main page
                    Title = "Fala";
                    ScanWave = "Skanuj obraz fali";
                    NoCamera = "Brak kamery";                    
                    CameraNotAvailable = ":( Brak dostępu do kamery.";
                    OK = "OK";
                    MakingPhotoHints = "Wskazówki do robienia zdjęcia";
                    Hints = "Ustaw swojego smartfona poziomo. " +
                        "Postaraj się, żeby cały obrazek mieścił się w kadrze i jednocześnie zajmował jak największą powierzchnię ekranu. " +
                        "Tło powinno być w miarę jasne i jednolite.";
                    ServerUnavailable = "Serwer niedostępny";
                    TryAgainLater = "Spróbuj ponownie później";
                    LogIn = "Zaloguj się";
                    LogOut = "Wyloguj się";
                    LoggedOut = "Wylogowano";
                    YouAreLoggedIn = "Jesteś teraz zalogowany na swoim koncie";
                    YouAreLoggedOut = "Jesteś teraz wylogowany ze swojego konta";
                    ManageAudios = "Zarządzaj nagraniami";

                    //View page
                    WavePreview = "Podgląd fali";
                    AudioNotFound = "Dźwięk nie znaleziony";
                    TryToMakePictureMoreAccurate = "Spróbuj zrobić zdjęcie dokładniej";

                    //Login page
                    CreateAccount = "Utwórz konto";
                    EnterData = "Wprowadź dane";
                    Username = "Nazwa użytkownika";
                    Password = "Hasło";
                    ValidationError = "Błąd";
                    UsernamePasswordRequired = "Nazwa użytkownika i hasło są wymagane";
                    UserNotFound = "Nie znaleziono użytkownika";
                    TryAgain = "Spróbuj ponownie";
                    WrongPassword = "Hasło niepoprawne";

                    //Create account page
                    PasswordsDoNotMatch = "Hasła nie są jednakowe";
                    Cancel = "Anuluj";
                    ChooseUsername = "Wybierz nazwę użytkownika";
                    ReEnterPassword = "Podaj hasło ponownie";
                    UserAlreadyExists = "Użytkownik już istnieje";
                    AccountCreated = "Konto zostało utworzone";
                    NowYouCanLogIn = "Możesz się teraz zalogować do swojego konta";

                    //Audio list
                    AudioList = "Lista nagrań audio";

                    //Audio item
                    Placeholder = "nazwa fali";
                    IsActive = "Jest aktywny";
                    StopRecordingAfterSilence = "Zatrzymać nagrywanie po chwili ciszy?";
                    Record = "Nagraj dźwięk";
                    Save = "Zapisz";
                    Play = "Odtwórz dźwięk";
                    View = "Pokaż obraz";
                    Delete = "Usuń";
                    StopRecording = "Zatrzymaj nagrywanie";
                    NameRequired = "Nazwa wymagana";
                    PleaseEnterName = "Proszę wprowadzić nazwę nagrania";
                    Cancelled = "Anulowano";
                    PaymentAccepted = "Płatność zaakceptowana";
                    ItemAlreadyExists = "Nagranie o tej nazwie już istnieje";
                    TryGivingDifferentName = "Spróbuj nadać inną nazwę";
                    PictureFileLocation = "Lokalizacja pliku obrazu";
                    AudioFileLocation = "Lokalizacja pliku audio";
                    Confirm = "Potwierdź";
                    DoYouReallyWantToRemove = "Czy naprawdę chcesz usunąć tę pozycję?";
                    ItemSaved = "Zapisano";
                    WaveAvailableForScanning = "Obrazek jest już widoczny dla skanera";

                    break;
            }
        }
	}
}
