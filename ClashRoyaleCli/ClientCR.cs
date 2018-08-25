using ClashRoyale.API;
using System;
using System.Linq;
using System.Collections.Generic;
using ClashRoyale.API.Models;
using System.Globalization;

namespace ClashRoyalCli
{

    public class ClientCR
    {
        #region Config
        const string BaseUrl = "https://api.clashroyale.com/v1";
        const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
        private const int MaxItems = 1152;
        private Uri _uriBaseUrl = new Uri(BaseUrl);
        private BearerCredentials _credentials;

        public static ClientCR Instance => new ClientCR();

        public ClientCR()
        {
            ConfigRepo.Load();
            _credentials = new BearerCredentials(ConfigRepo.Config.Token);
        }

        public bool NotConfigure()
        {
            return string.IsNullOrEmpty(ConfigRepo.Config.Token);
        }

        public void SetToken(string token)
        {
            ConfigRepo.Config.Token = token;
            _credentials = new BearerCredentials(ConfigRepo.Config.Token);
            ConfigRepo.Save();
        }

        public void SetClanTag(string tag)
        {
            if (!tag.StartsWith("#")) tag = "#" + tag;
            ConfigRepo.Config.ClanTag = tag;
            ConfigRepo.Save();
        }

        public void SetPlayertag(string tag)
        {
            if (!tag.StartsWith("#")) tag = "#" + tag;
            ConfigRepo.Config.PlayerTag = tag;
            ConfigRepo.Save();
        }
        #endregion

        #region Tournament
        public IEnumerable<TournamentBaseItemsItem> GetTournaments()
        {
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                foreach(var car1 in Alphabet)
                {
                    var tournament = client.SearchTournaments($"{car1}");
                    foreach (var item in tournament.Items)
                    {
                        if (TournamentIsFree(item))
                            yield return item;
                    }
                    //foreach (var car2 in Alphabet)
                    //{
                    //    tournament = client.SearchTournaments($"{car1}{car2}");
                    //    foreach (var item in tournament.Items)
                    //    {
                    //        if (TournamentIsFree(item))
                    //            yield return item;
                    //    }
                    //}
                }
            }
        }

        private static bool TournamentIsFree(TournamentBaseItemsItem item)
        {
            var test =  DateTime.Now.Subtract(item.CreatedTime).TotalMinutes;
            return (item.Type != "passwordProtected" && item.MaxCapacity - item.Capacity > 0);
                //|| (item.Status != "inProgress" && item.Type != "passwordProtected" && /*DateTime.Now.Subtract(item.CreatedTime).TotalMinutes > 20 && */item.MaxCapacity - item.Capacity > 0);
        }
        #endregion

        #region Player
        public PlayerDetail GetPlayer(string tag = null)
        {
            if (tag == null) tag = ConfigRepo.Config.PlayerTag;
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                return client.GetPlayer(tag);
            }
        }

        public List<MissingCard> GetMissingCards()
        {
            var player = Instance.GetPlayer();
            var missingCards = new List<MissingCard>();
            foreach (var card in player.Cards)
            {
                missingCards.Add(new MissingCard(card));
            }
            return missingCards;
        }

        #endregion

        #region Clan
        public Clan GetClan(string tag = null)
        {
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                return client.GetClan(tag);
            }
        }

        public int GetClanRank(string tag = null)
        {
            if (tag == null) tag = ConfigRepo.Config.ClanTag;
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                var clan = client.GetClan(tag);
                var listClan = GetClansRank(clan.ClanScore.Value - 50, clan.Location.Id);
                var posi = 0;
                foreach (var clantri in listClan)
                {
                    posi++;
                    if (clantri.Tag == clan.Tag) break;
                }
                return posi;
            }
        }

        public List<SearchResultClan> GetClansRank(int startTrophes, int? locationId)
        {
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                var dico = new Dictionary<string, SearchResultClan>();
                var wait = 2;
                var waitEnd = wait;

                while (waitEnd > 0)
                {
                    var clans = client.SearchClans(locationId: locationId, minScore: startTrophes);
                    foreach (var clanfind in clans.Items)
                    {
                        if(!dico.ContainsKey(clanfind.Tag))
                        {
                            dico.Add(clanfind.Tag, clanfind);
                        }
                    }
                    waitEnd = clans.Items.Count < MaxItems ? waitEnd - 1 : wait;
                    startTrophes += 50;
                }
                return dico.Select(p=> p.Value).OrderByDescending(p=> p.ClanScore).ToList();
            }
        }
        #endregion

        #region Location
        public Location GetLocation(string countryName)
        {
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                var locations = client.GetLocations();
                return locations.Items.FirstOrDefault(p => p.Name == countryName);
            }
        }
        #endregion
    }
}
