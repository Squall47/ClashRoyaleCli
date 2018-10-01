using ClashRoyaleApi.API.Models;
using MongoDB.Bson.Serialization;

namespace ClashRoyaleData
{
    public class RepoClan : RepoMongo<ClanBase, string>
    {
        static RepoClan()
        {
            BsonClassMap.RegisterClassMap<ClanBase>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(p=> p.Tag);
            });
        }

        public void Test(ClanBase item)
        {

        }
    }
}
