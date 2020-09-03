using System.Collections.Generic;
using WaveApi.Models;

namespace WaveApi.Interfaces
{
    public interface IItemDataRepository
    {
        //bool DoesItemExist(string id);
        ItemData All(string ID);
        //ItemData Find(string id);
        void Insert(ItemData data);
        void Update(ItemData data);
        void Delete(string id);
    }
}