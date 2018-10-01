using ClashRoyaleApi.API.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClashRoyaleData
{
    public static class Application
    {

        public static void Test()
        {
            var ctx = new ClashRoyaleDataContext("mongodb://localhost:27017", "clashroyale");

            ctx.RepoClan.DeleteAll();
            ctx.RepoClan.Insert(new ClanBase() { Tag = "45", Name = "zzzz", BadgeId = 4 });
            var clan = new ClanBase() { Tag = "48", Name = "ttttt", BadgeId = 4 };

            ctx.RepoClan.UpdateInsert(clan);
            var res = ctx.RepoClan.Get("45").FirstOrDefault();
        }
    }
}
