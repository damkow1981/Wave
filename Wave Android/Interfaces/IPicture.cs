namespace Wave
{
    public interface IPicture
    {
        string SavePictureToDisk(string filename, byte[] imageData);
    }
}
