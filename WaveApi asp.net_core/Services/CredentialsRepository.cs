using System.IO;
using System.Collections.Generic;
using System.Linq;
using WaveApi.Interfaces;
using WaveApi.Models;
using Newtonsoft.Json;


namespace WaveApi.Services
{
    public class CredentialsRepository : ICredentialsRepository
    {
        private List<Credentials> _credentialList;        

        public CredentialsRepository()
        {
            InitializeData();
        }

        public bool Get { get; private set; }

        public bool DoesItemExist(string username)
        {
            return _credentialList.Any(credentials => credentials.username == username);
        }

        public Credentials Find(string username)
        {
            return _credentialList.FirstOrDefault(credentials => credentials.username == username);
        }

        //wykonuje się przy tworzeniu konta
        public void Post(Credentials credentials)
        {
            //dodaje nową pozycję do listy
            _credentialList.Add(credentials);

            //zapisz listę loginów i haseł
            var json = JsonConvert.SerializeObject(_credentialList, Formatting.Indented);
            File.WriteAllText(Paths.LoginDataPath, json);
        }

        //wykonuje się przy logowaniu
        public void Put(Credentials credentials)
        {
            var _credentials = this.Find(credentials.username);

            if (_credentials.password == credentials.password)
            {
                Get = true;
            }
            else
            {
                Get = false;
            }
        }

        public void Delete(string id)
        {
            _credentialList.Remove(this.Find(id));
        }

        private void InitializeData()
        {
            _credentialList = new List<Credentials>();

            var credentials = new Credentials
            {
                username = "u",
                password = "p"
            };     

            //jeśli plik LoginData nie istnieje, to go utwórz
            if (!File.Exists(Paths.LoginDataPath))
            {
                File.Create(Paths.LoginDataPath).Dispose();
                
                _credentialList.Add(credentials);

                //zapisz listę loginów i haseł
                var json = JsonConvert.SerializeObject(_credentialList, Formatting.Indented);
                File.WriteAllText(Paths.LoginDataPath, json);
            }

            //wczytaj dane logowania z pliku
            try
            {
                var content = File.ReadAllText(Paths.LoginDataPath);
                _credentialList = JsonConvert.DeserializeObject<List<Credentials>>(content);
            }
            catch { }
        }
    }
}