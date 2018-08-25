using ClashRoyale.API.Models;

namespace ClashRoyalCli
{
    public class MissingCard : PlayerDetailCardsItem
    {
        const string Max = "Max";
        const string NotMax = "NotMax";
        public MissingCard(PlayerDetailCardsItem card) : base(card.Name, card.Level, card.MaxLevel, card.Count, card.IconUrls)
        {
            Missing = CardHelper.MissingCards(card);
            CardType = CardHelper.CardType(card);
            IsMax = card.MaxLevel == card.Level;
        }
        public int Missing { get; set; }
        public string CardType { get; set; }

        public bool IsMax { get; set; }

        public override string ToString()
        {
            return $"{CardType} \t {MaxLevel}/{Level} \t {(IsMax?Max:NotMax)}\t: {Missing:+0000;-0000} = {Name}";
        }
    }
}
