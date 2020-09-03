using System.Collections.Generic;
using WaveApi.Models;

namespace WaveApi.Interfaces
{
    public interface IPhotoRepository
    {
        Photo Get { get; }
        void Post(Photo photo);
        void Put(Photo photo);
    }
}