using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using WaveApi.Interfaces;
using WaveApi.Models;
using Newtonsoft.Json;

namespace WaveApi.Services
{
    public class AudioListRepository : IAudioListRepository
    {
        //public static string ID;
        //public static bool Active;

        public AudioListRepository()
        {
            InitializeData();
        }

        public IEnumerable<AudioItem> All(string username)
        {   
            return AudioList(username);
        }

        public bool DoesItemExist(AudioItem audioItem)
        {
            List<AudioItem> audioList = AudioList(audioItem.Username);

            return audioList.Any(item => item.Name == audioItem.Name);
        }

        public AudioItem Find(AudioItem audioItem)
        {
            List<AudioItem> audioList = AudioList(audioItem.Username);

            return audioList.FirstOrDefault(item => item.Name == audioItem.Name);
        }

        public void Insert(AudioItem item)
        {
            item.ID = ImageFilesProcessing.NadajNumerPlikowi(AudioIDs.AudioIDsSet);

            //ID = item.ID;
            //Active = item.Active;

            List<AudioItem> audioList = AudioList(item.Username);

            //dodaje nową pozycję do listy
            audioList.Add(item);

            //zapisz listę plików audio użytkownika
            var json = JsonConvert.SerializeObject(audioList, Formatting.Indented);
            File.WriteAllText(Paths.usersAudioListsPath + item.Username + ".txt", json);
        }

        public void Update(AudioItem item)
        {
            //ID = item.ID;
            //Active = item.Active;

            List<AudioItem> audioList = AudioList(item.Username);

            var audioItem = audioList.FirstOrDefault(_item => _item.Name == item.Name);

            audioList.Remove(audioItem);

            audioList.Add(item);

            //zapisz listę plików audio użytkownika
            var json = JsonConvert.SerializeObject(audioList, Formatting.Indented);
            File.WriteAllText(Paths.usersAudioListsPath + item.Username + ".txt", json);
        }

        public void Delete(AudioItem item)
        {
            List<AudioItem> audioList = AudioList(item.Username);

            var audioItem = audioList.FirstOrDefault(_item => _item.Name == item.Name);

            audioList.Remove(audioItem);

            //zapisz listę plików audio użytkownika
            var json = JsonConvert.SerializeObject(audioList, Formatting.Indented);
            File.WriteAllText(Paths.usersAudioListsPath + item.Username + ".txt", json);
        }

        private void InitializeData()
        { 
            AudioIDs.wczytajIDsSet();
        }

        private List<AudioItem> AudioList (string username)
        {
            List<AudioItem> audioList = new List<AudioItem>();

            try
            {
                string userAudioListPath = Paths.usersAudioListsPath + username + ".txt";

                //wczytaj listę audio użytkownika
                if (username != null && File.Exists(userAudioListPath))
                {
                    var content = File.ReadAllText(userAudioListPath);
                    audioList = JsonConvert.DeserializeObject<List<AudioItem>>(content);
                }
            }
            catch
            {
            }
            return audioList;
        }
    }
}