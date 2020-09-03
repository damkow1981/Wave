using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudioListREST
{
	public class AudioItemsManager
	{
		IRestServiceAudio restService;

		public AudioItemsManager (IRestServiceAudio service)
		{
			restService = service;
		}

		public Task<List<AudioItem>> GetTasksAsync (string username)
		{
			return restService.RefreshDataAsync (username);	
		}

		public Task SaveTaskAsync (AudioItem item, bool isNewItem = false)
		{
			return restService.SaveItemAsync (item, isNewItem);
		}

		public Task DeleteTaskAsync (AudioItem item)
		{
			return restService.DeleteItemAsync (item);
		}
	}
}
