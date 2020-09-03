namespace WaveApi.Models
{
    public class ItemData
    {
        public string ID { get; set; }

        public bool Active { get; set; }

        public byte[] AudioFile { get; set; }

        public byte[] WavePicture { get; set; }
    }
}