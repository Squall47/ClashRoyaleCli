using System.Collections.Generic;
using System.Linq;

namespace ClashRoyalCli.APIExtend.Models
{
    public class CardStat
    {
        public string Name { get; set; }
        public int Rank { get; set; }
        public int Count { get; set; }
        public IList<CardStat> AssociatedCards { get; set; }

        public override string ToString()
        {
            var asso = AssociatedCards != null ? " > " + string.Join(",", AssociatedCards) : "";
            return $"{Count.ToString().PadLeft(4)}={Name}{asso}";
        }

        private string assoCard => $"{Count}*{Name}";

        public string AssoCard(int posi)
        {
            return (posi < AssociatedCards.Count) ? AssociatedCards[posi].assoCard : "";
        }
    }
}
