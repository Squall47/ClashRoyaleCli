using ClashRoyale.API;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ClashRoyalCli
{
    partial class Program
    {

        static void Main(string[] args)
        {
            if (ClientCR.Instance.NotConfigure())
            {
                Console.WriteLine("You must complete the config.json file.");
                Console.WriteLine("Create an account on https://developer.clashroyale.com and genrate a key(token)");
                Console.ReadKey();
                return;
            }
            if(!string.IsNullOrEmpty(ConfigRepo.Config.PlayerTag))
            {
                var player = ClientCR.Instance.GetPlayer(ConfigRepo.Config.PlayerTag);
                Console.WriteLine($"Player : {player}");
            }
            if (!string.IsNullOrEmpty(ConfigRepo.Config.ClanTag))
            {
                var clan = ClientCR.Instance.GetClan(ConfigRepo.Config.ClanTag);
                Console.WriteLine($"Clan   : {clan}");
            }
            Console.WriteLine();
            while (true)
            {
                Console.WriteLine("1 - Rank of your clan");
                Console.WriteLine("2 - Open tournaments");
                Console.WriteLine("3 - Completed cards");
                Console.WriteLine("4 - Missing cards");

                Console.WriteLine("9 - Change player and clan");
                Console.WriteLine("x - End");

                Console.WriteLine();
                Console.Write("---> Choice : ");
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.KeyChar == '1')
                {
                    var posi = ClientCR.Instance.GetClanRank();
                    Console.WriteLine($"Rank of your clan is {posi}");
                }
                else if (key.KeyChar == '2')
                {
                    var tournements = ClientCR.Instance.GetTournaments().OrderBy(p => p.CreatedTime);
                    foreach (var item in tournements)
                    {
                        Console.WriteLine($"{item}");
                    }
                }
                else if (key.KeyChar == '3')
                {
                    var cards = ClientCR.Instance.GetMissingCards().Where(p=>p.Missing >= 0).ToList() ;
                    Console.WriteLine($">> {cards.Count} full collected cards");
                    Console.WriteLine($">> {cards.Where(p=> p.IsMax).Count()} max cards");
                    Console.WriteLine($"Type \t Max \t Cards \t Name");
                    foreach (var cardtype in CardHelper.CardLevel)
                    {
                        foreach (var card in cards.Where(p => p.CardType == cardtype).OrderBy(p => p.Missing))
                        {
                            Console.WriteLine($"{card}");
                        }
                    }
                }
                else if (key.KeyChar == '4')
                {
                    var cards = ClientCR.Instance.GetMissingCards();
                    Console.WriteLine($"Type \t Max \t Cards \t Name");
                    foreach (var cardtype in CardHelper.CardLevel)
                    {
                        foreach (var card in cards.Where(p => p.CardType == cardtype && p.Missing < 0).OrderByDescending(p => p.Missing))
                        {
                            Console.WriteLine($"{card}");
                        }
                    }
                }
                else if (key.KeyChar == '9')
                {
                    Console.WriteLine();
                    Console.WriteLine("1 - Change player and clan");
                    Console.WriteLine("2 - Change clan");
                    Console.WriteLine();
                    Console.Write("---> Choice : ");

                    var change = Console.ReadKey();
                    Console.WriteLine();
                    Console.WriteLine();
                    if (change.KeyChar == '1')
                    {
                        Console.Write($"Player tag :");
                        var tag = Console.ReadLine();
                        if (!tag.StartsWith("#")) tag = "#" + tag;
                        var player = ClientCR.Instance.GetPlayer(tag);
                        if (player != null)
                        {
                            Console.WriteLine($"Player : {player.Name}");
                            ClientCR.Instance.SetPlayertag(player.Tag);
                            Console.Write($"Change clan (y,n) :");
                            while (true)
                            {
                                var ok = Console.ReadKey();
                                Console.WriteLine();
                                if (ok.KeyChar.ToString().ToUpper() == "Y")
                                {
                                    Console.WriteLine($"Clan : {player.Clan.Name}");
                                    ClientCR.Instance.SetClanTag(player.Clan.Tag);
                                    break;
                                }
                                if (ok.KeyChar.ToString().ToUpper() == "N") break;
                            }
                        }
                        else
                        {
                            Console.Write($"Not exist");
                        }
                    }
                    else if (change.KeyChar == '2')
                    {
                        Console.Write($"Clan tag :");
                        var tag = Console.ReadLine();
                        if (!tag.StartsWith("#")) tag = "#" + tag;
                        var clan = ClientCR.Instance.GetClan(tag);
                        if (clan != null)
                        {
                            Console.WriteLine($"Clan : {clan.Name}");
                            ClientCR.Instance.SetClanTag(tag);
                        }
                        else
                        {
                            Console.Write($"Not exist");
                        }
                    }
                }
                else if (key.KeyChar == 'x')
                {
                    break;
                }
                Console.WriteLine();
            }
        }
    }
}
