namespace Wave
{
	public static class Constants
	{
        // URL of REST service
        public static string RestUrl = "http://damkow.dynu.net:5000/api/";
        //public static string RestUrl = "http://192.168.43.94:5000/api/";    //Red Bull
        //public static string RestUrl = "http://192.168.43.184:5000/api/";    //T-Mobile
        //public static string RestUrl = "http://192.168.0.10:5000/api/";    //Kowalscy
        public static string RestUrlAudio = RestUrl + "audioitemslist/{0}";
        public static string RestUrlData = RestUrl + "itemdata/{0}";
        public static string RestUrlLogin = RestUrl + "credentials/{0}";
        public static string RestUrlPhoto = RestUrl + "photo/{0}";

        // Credentials that are hard coded into the REST service
        public static string Username = "Xamarin";
		public static string Password = "Pa$$w0rd";
	}
}
