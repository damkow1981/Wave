using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudioListREST
{
	public interface IRestServiceAudio
	{
		Task<List<AudioItem>> RefreshDataAsync (string username);

		Task SaveItemAsync (AudioItem item, bool isNewItem);

		Task DeleteItemAsync (AudioItem item);
	}
}
