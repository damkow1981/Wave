using System.Threading.Tasks;

namespace DataREST
{
	public class DataManager
    {
		IRestServiceData restService;

		public DataManager(IRestServiceData service)
		{
			restService = service;
		}

		public Task<ItemData> GetTasksAsync (string ID)
		{
			return restService.RefreshDataAsync (ID);	
		}

		public Task SaveTaskAsync (ItemData data, bool isNewItem = false)
		{
			return restService.SaveDataAsync(data, isNewItem);
		}

		public Task DeleteTaskAsync (string ID)
		{
			return restService.DeleteDataAsync(ID);
		}
	}
}
