using System.Collections.Generic;
using WaveApi.Models;

namespace WaveApi.Interfaces
{
    public interface IAudioListRepository
    {
        bool DoesItemExist(AudioItem audioItem);
        IEnumerable<AudioItem> All(string username);
        AudioItem Find(AudioItem item);
        void Insert(AudioItem item);
        void Update(AudioItem item);
        void Delete(AudioItem item);
    }
}