namespace Wave
{
    public interface IAudio
    {
        string SaveAudioToDisk(string filename, byte[] audioData);
    }
}
