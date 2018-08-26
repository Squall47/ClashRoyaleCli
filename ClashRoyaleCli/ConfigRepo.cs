using System.IO;
using Newtonsoft.Json;

namespace ClashRoyalCli
{
    public static class ConfigRepo
    {
        const string FilePath = "config.json";
        public static CRConfig Config { get; set; }
        static ConfigRepo()
        {
            Load();
        }
        public static void Load()
        {
            if(!File.Exists(FilePath))
            {
                Save();
            }
            Config = JsonConvert.DeserializeObject<CRConfig>(File.ReadAllText(FilePath));
        }

        public static void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(Config));
        }

        public static bool NotConfigure()
        {
            return string.IsNullOrEmpty(ConfigRepo.Config.Token);
        }

        public static void SetClanTag(string tag)
        {
            if (!tag.StartsWith("#")) tag = "#" + tag;
            Config.ClanTag = tag;
            Save();
        }

        public static void SetPlayertag(string tag)
        {
            if (!tag.StartsWith("#")) tag = "#" + tag;
            Config.PlayerTag = tag;
            Save();
        }
    }
}
