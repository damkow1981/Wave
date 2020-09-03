using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using WaveApi.Services;

namespace WaveApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:5000")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            InitializeData();
            
            host.Run();
        }

        private static void InitializeData()
        {
            //utwórz katalog roboczy, jeśli nie istnieje
            DirectoryInfo workingDirectory = Directory.CreateDirectory(Paths.workingDirPath);

            try
            {
                //Set the current directory.
                Directory.SetCurrentDirectory(Paths.workingDirPath);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("The specified directory does not exist. {0}", e);
            }
            // Print to console the results.
            Console.WriteLine("\nRoot directory: {0}", Directory.GetDirectoryRoot(Paths.workingDirPath));
            Console.WriteLine("\nCurrent directory: {0}\n", Directory.GetCurrentDirectory());

            //utwórz katalog dla plików audio, jeśli nie istnieje
            DirectoryInfo usersAudioListsDirectory = Directory.CreateDirectory(Paths.usersAudioListsPath);

            //utwórz katalog dla plików audio, jeśli nie istnieje
            DirectoryInfo audiosDirectory = Directory.CreateDirectory(Paths.audiosPath);

            //utwórz katalog dla obrazów wave, jeśli nie istnieje
            DirectoryInfo wavesDirectory = Directory.CreateDirectory(Paths.wavesPath);

            //utwórz katalog dla zdjęcia, jeśli nie istnieje
            DirectoryInfo photoDirectory = Directory.CreateDirectory(Paths.photoPath);

            //jeśli plik HashSet nie istnieje, to go utwórz
            if (!File.Exists(Paths.HashSetPath))
            {
                File.Create(Paths.HashSetPath).Dispose();
            }

            //wczytaj obrazki z dysku do hash zestawu
            WavePictures.wczytajObrazki();
        }
    }
}
