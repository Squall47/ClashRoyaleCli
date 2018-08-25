using ClashRoyale.API.Models;

namespace ClashRoyalCli
{
    public static class CardHelper
    {
        public static int[] CardNumber = new int[] { 1, 2 , 4, 10, 20, 50, 100, 200, 400, 800, 1000, 2000, 5000 };

        public static string[] CardLevel = new string[] { "Commun" , "Rare" , "Epic" , "Legend" };

        public static int MissingCards(PlayerDetailCardsItem card)
        {
            var nbCardsCollect = 0;
            for (int i = card.Level.Value; i < card.MaxLevel; i++)
            {
                nbCardsCollect += CardNumber[i];
            }
            nbCardsCollect -= card.Count.Value;

            return nbCardsCollect * -1;
        }

        public static string CardType(PlayerDetailCardsItem card)
        {
            switch (card.MaxLevel)
            {
                case 5:
                    return CardLevel[3];
                case 8:
                    return CardLevel[2];
                case 11:
                    return CardLevel[1];
                case 13:
                    return CardLevel[0];
                default:
                    return "Undefined";
            }
        }
    }
}
