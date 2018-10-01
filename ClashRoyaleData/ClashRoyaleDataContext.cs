using ClashRoyaleApi.API.Models;

namespace ClashRoyaleData
{
    public class ClashRoyaleDataContext
    {
        private MongoDataContext _momgoDb;

        public RepoClan RepoClan { get; }

        public ClashRoyaleDataContext(string mongoCnx, string databaseName)
        {
            _momgoDb = new MongoDataContext(mongoCnx, databaseName);
            RepoClan = _momgoDb.Repository<RepoClan, ClanBase, string>(p => p.Tag);
        }
    }
}
