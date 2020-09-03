using System;
using WaveApi.Services;

namespace WaveApi
{
    public static class Paths
    {
        public static readonly string outputPath = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
        public static readonly string workingDirName = "Wave\\";
        public static readonly string workingDirPath = outputPath + workingDirName;

        public static readonly string audiosPath = outputPath + workingDirName + "audios\\";    //'outputPath + workingDirName' można pominąć
        public static readonly string wavesPath = outputPath + workingDirName + "waves\\";
        public static readonly string photoPath = outputPath + workingDirName + "photo\\";
        public static readonly string usersAudioListsPath = outputPath + workingDirName + "users_lists\\";

        public static readonly string HashSetPath = workingDirPath + "Hashset.txt";
        public static readonly string LoginDataPath = workingDirPath + "LoginData.txt";
    }
}
