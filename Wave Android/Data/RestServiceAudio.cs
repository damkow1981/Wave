using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Wave;


namespace AudioListREST
{
	public class RestServiceAudio : IRestServiceAudio
	{
		HttpClient client;

        public static HttpResponseMessage response;

        public RestServiceAudio ()
		{
			var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
			var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

			client = new HttpClient ();
			client.MaxResponseContentBufferSize = 2560000000000000000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
		}

		public async Task<List<AudioItem>> RefreshDataAsync (string username)
		{
            var Items = new List<AudioItem> ();

			// RestUrl = http://developer.xamarin.com:8081/api/todoitems
			var uri = new Uri (string.Format (Constants.RestUrlAudio, username));

			try {
				response = await client.GetAsync (uri);

                if (response.IsSuccessStatusCode)
                {
					var content = await response.Content.ReadAsStringAsync ();
					Items = JsonConvert.DeserializeObject <List<AudioItem>> (content);
				}
			}

            catch (Exception ex)
            {
                Debug.WriteLine (@"				ERROR {0}", ex.Message);
            }

			return Items;
		}

		public async Task SaveItemAsync (AudioItem item, bool isNewItem = false)
		{
			// RestUrl = http://developer.xamarin.com:8081/api/todoitems
			var uri = new Uri (string.Format (Constants.RestUrlAudio, string.Empty));

			try
            {
                item.ToBeDeleted = false;

                var json = JsonConvert.SerializeObject (item);
				var content = new StringContent (json, Encoding.UTF8, "application/json");

				response = null;
				if (isNewItem)
                {
					response = await client.PostAsync (uri, content);
				} else
                {
					response = await client.PutAsync (uri, content);
				}
				
				if (response.IsSuccessStatusCode)
                {
					Debug.WriteLine (@"				Item successfully saved.");
				}
				
			} catch (Exception ex) {
				Debug.WriteLine (@"				ERROR {0}", ex.Message);
			}
		}

		public async Task DeleteItemAsync (AudioItem item)
		{
            // RestUrl = http://developer.xamarin.com:8081/api/todoitems/{0}
            var uri = new Uri (string.Format (Constants.RestUrlAudio, string.Empty));

            try
            {
                item.ToBeDeleted = true;

                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                response = await client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
					Debug.WriteLine (@"				Item successfully deleted.");	
				}
				
			} catch (Exception ex)
            {
				Debug.WriteLine (@"				ERROR {0}", ex.Message);
			}
		}
	}
}
