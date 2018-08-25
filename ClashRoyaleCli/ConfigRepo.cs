using System.IO;
using Newtonsoft.Json;

namespace ClashRoyalCli
{
    public static class ConfigRepo
    {
        const string FilePath = "config.json";
        public static CRConfig Config { get; set; }
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
    }
}
