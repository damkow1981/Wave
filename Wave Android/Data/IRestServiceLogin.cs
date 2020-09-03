using System.Threading.Tasks;

namespace LoginPattern
{
	public interface IRestServiceLogin
	{
		Task<bool> RefreshDataAsync ();

		Task SaveCredentialsAsync(Credentials credentials, bool isNewItem);

		Task DeleteCredentialsItemAsync (string id);
	}
}
