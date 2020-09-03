using System.Collections.Generic;
using WaveApi.Models;

namespace WaveApi.Interfaces
{
    public interface ICredentialsRepository
    {
        bool DoesItemExist(string id);
        bool Get { get; }
        Credentials Find(string username);
        void Post(Credentials credentials);
        void Put(Credentials credentials);
        void Delete(string username);
    }
}