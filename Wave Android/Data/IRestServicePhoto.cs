using System.Threading.Tasks;

namespace PhotoREST
{
	public interface IRestServicePhoto
	{
		Task<Photo> RefreshDataAsync ();

		Task SavePhotoAsync (Photo photo, bool isNewItem);

		Task DeletePhotoAsync ();
	}
}
