using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClashRoyaleApi;
using ClashRoyaleApi.API;
using ClashRoyaleApi.API.Models;
using ClashRoyaleApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClashRoyaleWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClashRoyaleController : ControllerBase
    {
        [HttpGet("[action]")]
        public IEnumerable<TournamentItem> Tournois()
        {
            var client = new ClientCR(WebConfig.CRConfig);
            var data = client.GetTournaments().OrderBy(p => p.CreatedTime);
            //var data = Test.Create(5);
            return data;
        }

        [HttpGet("[action]")]
        public IEnumerable<CardDeck> Cards()
        {
            var client = new ClientCR(WebConfig.CRConfig);
            var data = client.GetMissingCards().OrderByDescending(p=> p.Cards);
            //var data = Test.Create(5);
            return data;
        }
    }


    
    public class Test
    {
        public string Name
        {
            get
            {
                return $"{WebConfig.Count}-Name";
            }
        }
        public static List<Test> Create(int nb)
        {
            var result = new List<Test>();
            for (int i = 0; i < nb; i++)
            {
                result.Add(new Test());
            }
            return result;
        }
    }
}
