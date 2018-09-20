using System;
using System.IO;
using ClashRoyaleApi;
using ClashRoyaleApi.Models;
using Newtonsoft.Json;

namespace ClashRoyalCli
{
    public static class ConfigRepo
    {
        const string FilePath = "config_clashroyale.json";
        public static CRConfig Config { get; set; }
        static ConfigRepo()
        {
            Load();
        }
        public static void Load()
        {
            var uri = AppDomain.CurrentDomain.BaseDirectory.Split('\\');
            var pathtest = "";
            for (int i = 0; i < uri.Length; i++)
            {
                pathtest = Path.Combine(pathtest, uri[i] + "\\");
                var filejson = Path.Combine(pathtest, FilePath);
                if (File.Exists(filejson))
                {
                    Config = JsonConvert.DeserializeObject<CRConfig>(File.ReadAllText(filejson));
                    break;
                }
            }
            
            if (Config == null)
            {
                Config = new CRConfig();
                Save();
            }
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
