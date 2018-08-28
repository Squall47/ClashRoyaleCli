using ClashRoyale.API;
using System.Linq;
using ClashRoyale.API.Models;
using ClashRoyalCli.APIExtend;
using Microsoft.Rest;
using System;

namespace ClashRoyalCli
{
    partial class Program
    {
        public static ClientCR client = new ClientCR(ConfigRepo.Config);
        static void Main(string[] args)
        {
            ServiceClientTracing.IsEnabled = true;
            ServiceClientTracing.AddTracingInterceptor(new DebugTracer());

            if (ConfigRepo.NotConfigure())
            {
                Console.WriteLine("You must complete the config.json file.");
                Console.WriteLine("Create an account on https://developer.clashroyale.com and genrate a key(token)");
                Console.ReadKey();
                return;
            }

            if (client.Player != null)
            {
                Console.WriteLine($"Player : {client.Player}");
            }
            if (client.Clan != null)
            {
                Console.WriteLine($"Clan   : {client.Clan}");
            }

            Console
                .NewChoice(ConsoleKey.D1, "Rank of your clan", Console
                    .NewChoice(ConsoleKey.D1, "Local", RankLocalClan)
                    .NewChoice(ConsoleKey.D2, "General", RankGeneralClan))
                .NewChoice(ConsoleKey.D2, "Various functions", Console
                    .NewChoice(ConsoleKey.D1, "Upcomming chests", UpcommingChests)
                    .NewChoice(ConsoleKey.D2, "Open tournaments", OpenTournaments)
                    .NewChoice(ConsoleKey.D3, "Cards list", ListCards))
                .NewChoice(ConsoleKey.D3, "My cards", Console
                    .NewChoice(ConsoleKey.D1, "Completed cards", CompletedCards)
                    .NewChoice(ConsoleKey.D2, "Missing cards", MissingCards))
                .NewChoice(ConsoleKey.D4, "Stats players", Console
                    .NewChoice(ConsoleKey.D1, "Usage cards in local top 200", UsageCardsTop)
                    .NewChoice(ConsoleKey.D2, "Winrate card local top 200", WinrateCardTop)
                    .NewChoice(ConsoleKey.D3, "Winrate card by trophes", WinrateCardByClanTrophe))
                .NewChoice(ConsoleKey.D9, "Change player and clan", Console
                    .NewChoice(ConsoleKey.D1, "Change player and clan", SettingPlayerClan)
                    .NewChoice(ConsoleKey.D2, "Change just clan", SettingClan))
                .WaitKey(ConsoleKey.X, client);
        }

        private static void ListCards()
        {
            var cards = client.GetCards();
            Console.WriteTable(cards, p => p.CardType, p => p.Name, p => p.MaxLevel, p => p.IconUrls.Medium);
        }

        private static void RankLocalClan()
        {
            var posi = client.GetClanRank();
            Console.WriteLine($"Local rank of your clan is {posi}");
        }

        private static void RankGeneralClan()
        {
            var posi = client.GetClanRank(false);
            Console.WriteLine($"General rank of your clan is {posi}");
        }

        private static void UpcommingChests()
        {
            var chests = client.GetChests();
            Console.WriteTable(chests, p => p.Index, p => p.Name);
        }
        private static void OpenTournaments()
        {
            var tournements = client.GetTournaments().OrderBy(p => p.CreatedTime);
            Console.WriteTable(tournements, p => p.CreatedTime, p => p.Places, p => p.Status, p => p.Name);
        }
        private static void CompletedCards()
        {
            var cards = client.GetMissingCards().Where(p => p.Cards >= 0).ToList();
            Console.WriteLine($">> {cards.Count} full collected cards");
            Console.WriteLine($">> {cards.Where(p => p.IsMax).Count()} max cards");
            var lst = cards.OrderBy(p => p.MaxLevel).ThenBy(p => p.Cards);
            Console.WriteTable(lst, p => p.CardType, p => p.Level, p => p.MaxLevel, p => p.IsMax, p => p.Cards, p => p.Name);
        }
        private static void MissingCards()
        {
            var cards = client.GetMissingCards().Where(p => p.Cards < 0);
            var lst = cards.OrderBy(p => p.MaxLevel).ThenByDescending(p => p.Cards);
            Console.WriteTable(lst, p => p.CardType, p => p.Level, p => p.MaxLevel, p => p.IsMax, p => p.Cards, p => p.Name);
        }
        private static void UsageCardsTop()
        {
            var clanlocal = client.GetDetailClan();
            var cards = client.GetCarsUsageTopRanking(clanlocal.Location.Id);
            Console.WriteTable(cards, p => p.Rank, p => p.Name, p => p.Count, p => p.AssoCard(0), p => p.AssoCard(1), p => p.AssoCard(2)
            , p => p.AssoCard(3));
        }
        private static void WinrateCardTop()
        {
            var clanlocal = client.GetDetailClan();
            var cards = client.GetCardsWinTopPlayer(clanlocal.Location.Id);
            Console.WriteTable(cards, p => p.Rank, p => p.Name, p => p.Count);
        }
        private static void SettingClan()
        {
            Console.Write($"Clan tag :");
            var tag = Console.ReadLine();
            if (!tag.StartsWith("#")) tag = "#" + tag;
            var clanlocal = client.GetClan(tag);
            if (clanlocal != null)
            {
                Console.WriteLine($"Clan : {clanlocal.Name}");
                client.SetClanTag(tag);
                ConfigRepo.SetClanTag(tag);
            }
            else
            {
                Console.Write($"Not exist");
            }
        }
        private static void SettingPlayerClan()
        {
            Console.Write($"Player tag :");
            var tag = Console.ReadLine();
            if (!tag.StartsWith("#")) tag = "#" + tag;
            var playerlocal = client.GetPlayer(tag);
            if (playerlocal != null)
            {
                Console.WriteLine($"Player : {playerlocal.Name}");
                client.SetPlayertag(playerlocal.Tag);
                ConfigRepo.SetPlayertag(playerlocal.Tag);
                Console.WriteLine($"Clan : {playerlocal.Clan.Name}");
                client.SetClanTag(playerlocal.Clan.Tag);
                ConfigRepo.SetClanTag(playerlocal.Clan.Tag);
            }
            else
            {
                Console.Write($"Not exist");
            }

        }
        private static void WinrateCardByClanTrophe()
        {
            var cards = client.GetCardsWinByPlayerIntoClan(55000);
            Console.WriteTable(cards, p => p.Rank, p => p.Name, p => p.Count);
        }
    }
}
