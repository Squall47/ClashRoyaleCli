using ClashRoyale.API;
using System;
using System.Linq;
using System.Collections.Generic;
using ClashRoyale.API.Models;
using System.Globalization;
using ClashRoyalCli.APIExtend.Models;

namespace ClashRoyalCli.APIExtend
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
        public List<CardBase> Cards { get; private set; }
        public bool DemandStopping { get; set; }


        public ClientCR(CRConfig config)
        {
            Config = config;
            _credentials = new BearerCredentials(Config.Token);
            Player = GetPlayer();
            Clan = GetClan();
            Cards = GetCards();
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
            try
            {
                var tags = new Dictionary<string, string>();
                using (var client = new CRClient(_uriBaseUrl, _credentials))
                {
                    foreach (var car1 in Alphabet)
                    {
                        if (DemandStopping) break;
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
            finally
            {
                DemandStopping = false;
            }
        }

        private static bool TournamentIsFree(TournamentBaseItemsItem item)
        {
            return (item.Type != "passwordProtected" && item.MaxCapacity - item.Capacity > 0);
                //|| (item.Status != "inProgress" && item.Type != "passwordProtected" && /*DateTime.Now.Subtract(item.CreatedTime).TotalMinutes > 20 && */item.MaxCapacity - item.Capacity > 0);
        }
        #endregion

        #region Player

        public List<CardStat> GetCardsWinTopPlayer(int? idlocation = null)
        {
            var result = new List<CardStat>();
            try
            {
                using (var client = new CRClient(_uriBaseUrl, _credentials))
                {
                    var players = client.GetPlayerRanking(idlocation?.ToString());
                    result = InternalGetCardWin(players.Items.Cast<PlayerBase>().ToList());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                DemandStopping = false;
            }
            return result;
        }

        private List<CardStat> InternalGetCardWin(IList<PlayerBase> players)
        {
            var winners = new List<BattleLogItem>();
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                foreach (var player in players)
                {
                    if (DemandStopping) break;
                    var battles = client.GetPlayerBattles(player.Tag).Where(p => p.Type == "PvP");
                    foreach (var battle in battles)
                    {
                        if (winners.All(p=> !p.IsSame(battle)))
                        {
                            winners.Add(battle);
                        }
                    }
                }
                var cardsUsage = winners.SelectMany(p => p.Winners).SelectMany(p=> p.Cards).GroupBy(p => p.Name).Select(p => new CardStat { Name = p.First().Name, Count = p.Count(), Url = p.First().IconUrls.Medium }).OrderByDescending(p => p.Count).ToList();

                var posi = 0;
                var lastusage = int.MaxValue;
                foreach (var card in cardsUsage)
                {
                    if (card.Count != lastusage)
                    {
                        posi++;
                        lastusage = card.Count;
                    }
                    card.Rank = posi;
                }
                return cardsUsage;
            }
        }

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

        public List<CardBase> GetCards()
        {
            using (var client = new CRClient(_uriBaseUrl, _credentials))
            {
                return client.GetCards().Items.OrderBy(p=> p.MaxLevel).ToList();
            }
        }

        public List<CardDeck> GetMissingCards()
        {
            var missingCards = new List<CardDeck>();
            foreach (var card in Player.Cards)
            {
                missingCards.Add(new CardDeck(card));
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

        public List<CardStat> GetCardsWinByPlayerIntoClan(int startTrophes, int? locationId = null)
        {
            var result = new List<CardStat>();
            try
            {
                var cardwins = new List<CardStat>();
                var clans = GetClansRank(startTrophes, locationId);
                using (var client = new CRClient(_uriBaseUrl, _credentials))
                {
                    foreach (var clan in clans)
                    {
                        if (DemandStopping) break;
                        var clanDetail = client.GetClan(clan.Tag);
                        var cardwin = InternalGetCardWin(clanDetail.MemberList.Cast<PlayerBase>().ToList());
                        cardwins.AddRange(cardwin);
                    }
                }

                result = cardwins.GroupBy(p => p.Name).Select(p => new CardStat { Name = p.First().Name, Count = p.Sum(c => c.Count), Url= p.First().Url }).OrderByDescending(p => p.Count).ToList();

                var posi = 0;
                var lastusage = int.MaxValue;
                foreach (var card in result)
                {
                    if (card.Count != lastusage)
                    {
                        posi++;
                        lastusage = card.Count;
                    }
                    card.Rank = posi;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                DemandStopping = false;
            }
            return result;
        }

        public List<SearchResultClan> GetClansRank(int startTrophes, int? locationId)
        {
            var result = new List<SearchResultClan>();
            try
            {
                using (var client = new CRClient(_uriBaseUrl, _credentials))
                {
                    var dico = new Dictionary<string, SearchResultClan>();
                    var wait = 2;
                    var waitEnd = wait;

                    while (waitEnd > 0)
                    {
                        if (DemandStopping) break;
                        var clans = client.SearchClans(locationId: locationId, minScore: startTrophes);
                        foreach (var clanfind in clans.Items)
                        {
                            if (!dico.ContainsKey(clanfind.Tag))
                            {
                                dico.Add(clanfind.Tag, clanfind);
                            }
                        }
                        waitEnd = clans.Items.Count < MaxItems ? waitEnd - 1 : wait;
                        startTrophes += 50;
                    }
                    result = dico.Select(p => p.Value).OrderByDescending(p => p.ClanScore).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                DemandStopping = false;
            }
            return result;
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
        public List<CardStat> GetCarsUsageTopRanking(int? idlocation = null, int nbcardAssociated = 8)
        {
            var playerCards = new List<PlayerDetail>();
            var cardsUsage = new List<CardStat>();
            try
            {
                
                using (var client = new CRClient(_uriBaseUrl, _credentials))
                {
                    var players = client.GetPlayerRanking(idlocation?.ToString());
                    foreach (var player in players.Items)
                    {
                        if (DemandStopping) break;
                        var playerDetail = client.GetPlayer(player.Tag);
                        if(playerDetail != null && playerDetail.CurrentCards !=null)
                            playerCards.Add(playerDetail);
                    }
                }
                cardsUsage = playerCards.SelectMany(p => p.CurrentCards).GroupBy(p => p.Name).Select(p => new CardStat { Name = p.First().Name, Count = p.Count(), Url = p.First().IconUrls.Medium }).OrderByDescending(p => p.Count).ToList();
                var posi = 0;
                var lastusage = int.MaxValue;
                foreach (var card in cardsUsage)
                {
                    if (card.Count != lastusage)
                    {
                        posi++;
                        lastusage = card.Count;
                    }
                    card.Rank = posi;
                    card.AssociatedCards = playerCards.Where(p => p.CurrentCards.Any(c => c.Name == card.Name)).SelectMany(p => p.CurrentCards).Where(p => p.Name != card.Name).GroupBy(p => p.Name).Select(p => new CardStat { Name = p.First().Name, Count = p.Count() , Url = p.First().IconUrls.Medium}).OrderByDescending(p => p.Count).Take(nbcardAssociated).ToList();
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                DemandStopping = false;
            }
            return cardsUsage;
        }
        #endregion
    }
}
