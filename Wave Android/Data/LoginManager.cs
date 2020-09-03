using System.Threading.Tasks;
using AudioListREST;

namespace LoginPattern
{
	public class LoginManager
	{
        IRestServiceLogin restService;

		public LoginManager(IRestServiceLogin service)
		{
			restService = service;
		}

		public Task<bool> GetTasksAsync ()
		{
			return restService.RefreshDataAsync ();	
		}

		public Task SaveTaskAsync (Credentials credentials, bool isNewItem = false)
		{
                return restService.SaveCredentialsAsync (credentials, isNewItem);
		}

		public Task DeleteTaskAsync (AudioItem item)
		{
			return restService.DeleteCredentialsItemAsync (item.Name);
		}
	}
}
