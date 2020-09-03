using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Wave;


namespace LoginPattern
{
	public class RestServiceLogin : IRestServiceLogin
    {
		HttpClient client;

        public static HttpResponseMessage response;

        public List<Credentials> Items { get; private set; }

        public bool credentials_confirmed { get; private set; }

        public RestServiceLogin ()
		{
			var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
			var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

			client = new HttpClient ();
			client.MaxResponseContentBufferSize = 2560000000000000000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
		}

		public async Task<bool> RefreshDataAsync ()
		{
            credentials_confirmed = new bool ();

			var uri = new Uri (string.Format (Constants.RestUrlLogin, string.Empty));

			try {
				response = await client.GetAsync (uri);

                if (response.IsSuccessStatusCode)
                {
					var content = await response.Content.ReadAsStringAsync ();
                    credentials_confirmed = JsonConvert.DeserializeObject <bool> (content);
				}
			}

            catch (Exception ex)
            {
                Debug.WriteLine (@"				ERROR {0}", ex.Message);
            }

			return credentials_confirmed;
		}

		public async Task SaveCredentialsAsync(Credentials credentials, bool isNewItem = false)
		{
			var uri = new Uri (string.Format (Constants.RestUrlLogin, string.Empty));

			try {
				var json = JsonConvert.SerializeObject (credentials);
				var content = new StringContent (json, Encoding.UTF8, "application/json");

				response = null;
				if (isNewItem)
                {
					response = await client.PostAsync (uri, content);
				}
                else
                {
					response = await client.PutAsync (uri, content);
				}
				
				if (response.IsSuccessStatusCode) {
					Debug.WriteLine (@"				Item successfully saved.");
				}
				
			} catch (Exception ex) {
				Debug.WriteLine (@"				ERROR {0}", ex.Message);
			}
		}

		public async Task DeleteCredentialsItemAsync (string id)
		{
			var uri = new Uri (string.Format (Constants.RestUrlLogin, id));

			try {
				response = await client.DeleteAsync (uri);

				if (response.IsSuccessStatusCode) {
					Debug.WriteLine (@"				Item successfully deleted.");	
				}
				
			} catch (Exception ex) {
				Debug.WriteLine (@"				ERROR {0}", ex.Message);
			}
		}
	}
}
