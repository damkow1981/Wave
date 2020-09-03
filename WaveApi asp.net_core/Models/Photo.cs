using System.ComponentModel.DataAnnotations;

namespace WaveApi.Models
{
    public class Photo
    {
        public string FileName { get; set; }

        public byte[] Picture { get; set; }

        public byte[] AudioFile { get; set; }
    }
}