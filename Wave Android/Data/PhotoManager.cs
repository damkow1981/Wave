using System.Threading.Tasks;

namespace PhotoREST
{
	public class PhotoManager
	{
		IRestServicePhoto restService;

		public PhotoManager(IRestServicePhoto service)
		{
			restService = service;
		}

		public Task<Photo> GetTasksAsync ()
		{
			return restService.RefreshDataAsync ();	
		}

		public Task SaveTaskAsync (Photo photo, bool isNewItem = false)
		{
			return restService.SavePhotoAsync(photo, isNewItem);
		}

		public Task DeleteTaskAsync (Photo wavePicture)
		{
			return restService.DeletePhotoAsync();
		}
	}
}
