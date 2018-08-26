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
        const string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789#@~-_{([])}|/\\*ç^€%ù¨='?,;.!:";
        private const int MaxItems = 1152;
        private Uri _uriBaseUrl = new Uri(BaseUrl);
        private BearerCredentials _credentials;
        public CRConfig Config { get; }
        public PlayerDetail Player { get; private set; }
        public Clan Clan { get; private set; }

        public ClientCR(CRConfig config)
        {
            Config = config;
            _credentials = new BearerCredentials(Config.Token);
            Player = GetPlayer();
            Clan = GetClan();
        }

        public void SetClanTag(string tag)
        {
            if (!tag.StartsWith("#")) tag = "#" + tag;
            Config.ClanTag = tag;
            Clan = GetClan();
        }

        public void SetPlayertag(string tag)
        {
            if (!tag.StartsWith("#")) tag = "#" + tag;
            Config.PlayerTag = tag;
            Player = GetPlayer();
        }
        #endregion

        #region Tournament
        public IEnumerable<TournamentBaseItemsItem> GetTournaments()
        {
            var tags = new Dictionary<string, string>();
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                foreach(var car1 in Alphabet)
                {
                    var tournament = client.SearchTournaments($"{car1}");
                    foreach (var item in tournament.Items)
                    {
                        if (TournamentIsFree(item))
                        {
                            if (!tags.ContainsKey(item.Tag))
                            {
                                tags.Add(item.Tag, null);
                                yield return item;
                            }
                        }
                    }
                }
            }
        }

        private static bool TournamentIsFree(TournamentBaseItemsItem item)
        {
            return (item.Type != "passwordProtected" && item.MaxCapacity - item.Capacity > 0);
                //|| (item.Status != "inProgress" && item.Type != "passwordProtected" && /*DateTime.Now.Subtract(item.CreatedTime).TotalMinutes > 20 && */item.MaxCapacity - item.Capacity > 0);
        }
        #endregion

        #region Player

        public List<UpcomingChestsListItemsItem> GetChests()
        {
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                return client.GetPlayerUpcomingChests(Config.PlayerTag).Items.ToList();
            }
        }

        public PlayerDetail GetPlayer(string tag = null)
        {
            tag = (tag == null)?Config.PlayerTag: (!tag.StartsWith("#"))? $"#{tag}":tag;
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                return client.GetPlayer(tag);
            }
        }

        public List<MissingCard> GetMissingCards()
        {
            var missingCards = new List<MissingCard>();
            foreach (var card in Player.Cards)
            {
                missingCards.Add(new MissingCard(card));
            }
            return missingCards;
        }

        #endregion

        #region Clan
        public Clan GetClan(string tag = null)
        {
            tag = (tag == null) ? Config.ClanTag : (!tag.StartsWith("#")) ? $"#{tag}" : tag;
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                return client.GetClan(tag);
            }
        }

        public SearchResultClan GetDetailClan()
        {
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                var clans = client.SearchClans(Clan.Name);
                return clans.Items.FirstOrDefault(p => p.Tag == Clan.Tag);
            }
        }

        public int GetClanRank(bool local=true)
        {
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                var listClan = GetClansRank(Clan.ClanScore.Value - 50, local?Clan.Location.Id:null);
                var posi = 0;
                foreach (var clantri in listClan)
                {
                    posi++;
                    if (clantri.Tag == Clan.Tag) break;
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
        public Location GetLocation(string countryName = null)
        {
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                var locations = client.GetLocations();
                return locations.Items.FirstOrDefault(p => p.Name == countryName);
            }
        }
        #endregion

        #region TopRanking
        public List<CardUsage> GetCarsUsageTopRanking(int? idlocation = null)
        {
            var playerCards = new List<PlayerDetail>();
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                var players = client.GetPlayerRanking(idlocation?.ToString());
                foreach (var player in players.Items)
                {
                    var playerDetail = client.GetPlayer(player.Tag);
                    playerCards.Add(playerDetail);
                }
            }
            return playerCards.SelectMany(p => p.CurrentCards).GroupBy(p=> p.Name).Select(p=>new CardUsage { Name = p.First().Name, UsageCount = p.Count() }).OrderByDescending(p=> p.UsageCount).ToList();
        }
        #endregion
    }
}
