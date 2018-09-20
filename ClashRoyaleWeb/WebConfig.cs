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
        const string FilePath = "config_clashroyale.json";
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
                    CRConfig = JsonConvert.DeserializeObject<CRConfig>(File.ReadAllText(filejson));
                    break;
                }
            }

            if (CRConfig == null)
            {
                CRConfig = new CRConfig();
            }
        }
    }
}
