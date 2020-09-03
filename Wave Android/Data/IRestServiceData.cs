using System.Threading.Tasks;

namespace DataREST
{
	public interface IRestServiceData
	{
		Task<ItemData> RefreshDataAsync (string ID);

		Task SaveDataAsync (ItemData data, bool isNewItem);

		Task DeleteDataAsync (string ID);
	}
}
