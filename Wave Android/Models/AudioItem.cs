namespace AudioListREST
{
	public class AudioItem
	{
        public string Username { get; set; }

        public string Name { get; set; }

        public string ID { get; set; }

        public bool Active { get; set; }

        public bool ToBeDeleted { get; set; }
    }
}
