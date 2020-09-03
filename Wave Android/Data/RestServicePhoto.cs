using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Wave;


namespace PhotoREST
{
	public class RestServicePhoto : IRestServicePhoto
    {
		HttpClient client;

        public static HttpResponseMessage response;

        public Photo photo { get; private set; }
        
        public RestServicePhoto()
		{
			var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
			var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

			client = new HttpClient ();
			client.MaxResponseContentBufferSize = 2560000000000000000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
		}

		public async Task<Photo> RefreshDataAsync ()
		{
            photo = new Photo();

			var uri = new Uri (string.Format (Constants.RestUrlPhoto, string.Empty));

			try {
				response = await client.GetAsync (uri);

                if (response.IsSuccessStatusCode)
                {
					var content = await response.Content.ReadAsStringAsync ();
                    photo = JsonConvert.DeserializeObject <Photo> (content);
				}
			}

            catch (Exception ex)
            {
                Debug.WriteLine (@"				ERROR {0}", ex.Message);                
            }

			return photo;
		}

		public async Task SavePhotoAsync (Photo photo, bool isNewItem = false)
		{
			var uri = new Uri (string.Format (Constants.RestUrlPhoto, string.Empty));

			try {
				var json = JsonConvert.SerializeObject (photo);
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
				
			}
            catch (Exception ex)
            {
				Debug.WriteLine (@"				ERROR {0}", ex.Message);
            }
		}

		public async Task DeletePhotoAsync ()
		{
			var uri = new Uri (string.Format (Constants.RestUrlPhoto, string.Empty));

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
