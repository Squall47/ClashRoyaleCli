using System.Linq;
using Microsoft.Rest;
using System;
using System.Linq.Expressions;
using System.IO;
using ClashRoyaleApi;

namespace ClashRoyalCli
{
    partial class Program
    {
        public static ClientCR client;
        public static ExtConsole ExtConsole = new ExtConsole();

        static void Main(string[] args)
        {
            ServiceClientTracing.IsEnabled = true;
            ServiceClientTracing.AddTracingInterceptor(new DebugTracer(ExtConsole));

            if (ConfigRepo.NotConfigure())
            {
                ExtConsole.WriteLine("You must complete the config.json file.");
                ExtConsole.WriteLine("Create an account on https://developer.clashroyale.com and genrate a key(token)");
                ExtConsole.ReadKey();
                return;
            }

            client = new ClientCR(ConfigRepo.Config, ExtConsole);

            if (client.Player != null)
            {
                ExtConsole.WriteLine($"Player : {client.Player}");
            }
            if (client.Clan != null)
            {
                ExtConsole.WriteLine($"Clan   : {client.Clan}");
            }

            ExtConsole
                .NewChoice(ConsoleKey.D1, "Rank of your clan", ExtConsole
                    .NewChoice(ConsoleKey.D1, "Local", RankLocalClan)
                    .NewChoice(ConsoleKey.D2, "General", RankGeneralClan))
                .NewChoice(ConsoleKey.D2, "Various functions", ExtConsole
                    .NewChoice(ConsoleKey.D1, "Upcomming chests", UpcommingChests)
                    .NewChoice(ConsoleKey.D2, "Open tournaments", OpenTournaments)
                    .NewChoice(ConsoleKey.D3, "Cards list", ListCards))
                .NewChoice(ConsoleKey.D3, "My cards", ExtConsole
                    .NewChoice(ConsoleKey.D1, "Completed cards", CompletedCards)
                    .NewChoice(ConsoleKey.D2, "Missing cards", MissingCards))
                .NewChoice(ConsoleKey.D4, "Stats players", ExtConsole
                    .NewChoice(ConsoleKey.D1, "Usage cards in local top 200", UsageCardsTop)
                    .NewChoice(ConsoleKey.D2, "Winrate card local top 200", WinrateCardTop)
                    .NewChoice(ConsoleKey.D3, "Winrate card by trophes (is long)", WinrateCardByClanTrophe, "Minimum clan trophy : "))
                .NewChoice(ConsoleKey.D9, "Change player and clan", ExtConsole
                    .NewChoice(ConsoleKey.D1, "Change player and clan", SettingPlayerClan)
                    .NewChoice(ConsoleKey.D2, "Change just clan", SettingClan))
                .WaitKey(ConsoleKey.X, client);
        }

        private static void ListCards(params string[] args)
        {
            var cards = client.GetCards();
            ExtConsole.WriteTable(cards, p => p.CardType, p => p.Name, p => p.MaxLevel, p => p.IconUrls.Medium);
            if (client.Config.SaveFiles)
                ExtConsole.SaveFile($"Cards", cards, p => p.CardType, p => p.Name, p => p.MaxLevel, p => p.Icon);
        }
        private static void RankLocalClan(params string[] args)
        {
            var posi = client.GetClanRank();
            ExtConsole.WriteLine($">> Local {client.Clan.Location.Name} rank of clan {client.Clan.Name} is {posi}");
        }
        private static void RankGeneralClan(params string[] args)
        {
            var posi = client.GetClanRank(false);
            ExtConsole.WriteLine($">> General rank of clan {client.Clan.Name}  is {posi}");
        }
        private static void UpcommingChests(params string[] args)
        {
            var chests = client.GetChests();
            ExtConsole.WriteTable(chests, p => p.Index, p => p.Name);
        }
        private static void OpenTournaments(params string[] args)
        {
            var tournements = client.GetTournaments().OrderBy(p => p.CreatedTime);
            ExtConsole.WriteTable(tournements, p => p.Start, p => p.Places, p => p.Status, p => p.Name);
        }
        private static void CompletedCards(params string[] args)
        {
            var cards = client.GetMissingCards().Where(p => p.Cards >= 0).ToList();
            ExtConsole.WriteLine($">> {cards.Count} full collected cards");
            ExtConsole.WriteLine($">> {cards.Where(p => p.IsMax).Count()} max cards");
            var lst = cards.OrderBy(p => p.MaxLevel).ThenBy(p => p.Cards);
            ExtConsole.WriteTable(lst, p => p.CardType, p => p.Level, p => p.MaxLevel, p => p.IsMax, p => p.Cards, p => p.Name);
        }
        private static void MissingCards(params string[] args)
        {
            var cards = client.GetMissingCards().Where(p => p.Cards < 0);
            var lst = cards.OrderBy(p => p.MaxLevel).ThenByDescending(p => p.Cards);
            ExtConsole.WriteTable(lst, p => p.CardType, p => p.Level, p => p.MaxLevel, p => p.IsMax, p => p.Cards, p => p.Name);
        }
        private static void UsageCardsTop(params string[] args)
        {
            ExtConsole.WriteLine($">> Usage cards top 200 {client.Clan.Location.Name}");
            var cards = client.GetCarsUsageTopRanking(client.Clan.Location.Id);
            ExtConsole.WriteTable(cards, p => p.Rank, p => p.Name, p => p.Count, p => p.AssoCard(0), p => p.AssoCard(1), p => p.AssoCard(2), p => p.AssoCard(3));
            if(client.Config.SaveFiles)
                ExtConsole.SaveFile($"UsageCardsTop-{client.Clan.Location.Name}", cards, p => p.Rank, p => p.Icon, p => p.Name, p => p.Count, p => p.AssoCard());
        }
        private static void WinrateCardTop(params string[] args)
        {
            ExtConsole.WriteLine($">> Win cards into 25 last battles for players top 200 {client.Clan.Location.Name}");
            var cards = client.GetCardsWinTopPlayer(client.Clan.Location.Id);
            ExtConsole.WriteTable(cards, p => p.Rank, p => p.Name, p => p.Count);
            if (client.Config.SaveFiles)
                ExtConsole.SaveFile($"WinsCardsTop-{client.Clan.Location.Name}", cards, p => p.Rank, p => p.Icon, p => p.Name, p => p.Count);
        }
        private static void WinrateCardByClanTrophe(params string[] args)
        {
            ExtConsole.WriteLine($">> Win cards into 25 last battles for players into clan trophies above {args[0]}");
            var cards = client.GetCardsWinByPlayerIntoClan(int.Parse(args[0]));
            ExtConsole.WriteTable(cards, p => p.Rank, p => p.Name, p => p.Count);
            if (client.Config.SaveFiles)
                ExtConsole.SaveFile($"WinsCardsGeneralPlayersIntoClanAbove-{args[0]}", cards, p => p.Rank, p => p.Icon, p => p.Name, p => p.Count);
        }

        private static void SettingClan(params string[] args)
        {
            ExtConsole.Write($"Clan tag :");
            var tag = ExtConsole.ReadLine();
            if (!tag.StartsWith("#")) tag = "#" + tag;
            var clanlocal = client.GetClan(tag);
            if (clanlocal != null)
            {
                ExtConsole.WriteLine($"Clan : {clanlocal.Name}");
                client.SetClanTag(tag);
                ConfigRepo.SetClanTag(tag);
            }
            else
            {
                ExtConsole.Write($"Not exist");
            }
        }
        private static void SettingPlayerClan(params string[] args)
        {
            ExtConsole.Write($"Player tag :");
            var tag = ExtConsole.ReadLine();
            if (!tag.StartsWith("#")) tag = "#" + tag;
            var playerlocal = client.GetPlayer(tag);
            if (playerlocal != null)
            {
                ExtConsole.WriteLine($"Player : {playerlocal.Name}");
                client.SetPlayertag(playerlocal.Tag);
                ConfigRepo.SetPlayertag(playerlocal.Tag);
                ExtConsole.WriteLine($"Clan : {playerlocal.Clan.Name}");
                client.SetClanTag(playerlocal.Clan.Tag);
                ConfigRepo.SetClanTag(playerlocal.Clan.Tag);
            }
            else
            {
                ExtConsole.Write($"Not exist");
            }
        }
    }
}
