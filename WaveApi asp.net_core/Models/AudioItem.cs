using System.ComponentModel.DataAnnotations;

namespace WaveApi.Models
{
    public class AudioItem
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        public string ID { get; set; }

        [Required]
        public bool Active { get; set; }

        public bool ToBeDeleted { get; set; }
    }
}