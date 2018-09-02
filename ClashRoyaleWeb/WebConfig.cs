using ClashRoyaleApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClashRoyaleWeb
{
    public static class WebConfig
    {
        const string FilePath = "config.json";
        const string SecretFilePath = "../../config.json";
        private static int _count = 0;
        public static int Count
        {
            get
            {
                return _count++;
            }
        }
        public static CRConfig CRConfig { get; private set; }
        static WebConfig()
        {
            if (File.Exists(SecretFilePath))
            {
                CRConfig = JsonConvert.DeserializeObject<CRConfig>(File.ReadAllText(SecretFilePath));
            }
            else
            {
                CRConfig = JsonConvert.DeserializeObject<CRConfig>(File.ReadAllText(FilePath));
            }
        }
    }
}
