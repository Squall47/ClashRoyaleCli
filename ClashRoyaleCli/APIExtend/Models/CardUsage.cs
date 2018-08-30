using ClashRoyale.API.Models;
using System.Collections.Generic;
using System.Linq;

namespace ClashRoyalCli.APIExtend.Models
{
    public class CardStat
    {
        public string Name { get; set; }
        public int Rank { get; set; }
        public int Count { get; set; }
        
        public List<CardStat> AssociatedCards { get; set; }

        public override string ToString()
        {
            var asso = AssociatedCards != null ? " > " + string.Join(",", AssociatedCards) : "";
            return $"{Count.ToString().PadLeft(4)}={Name}{asso}";
        }

        private string assoCard => $"{Count}*{Name}";

        public string Url { get; set; }

        public string AssoCard(int posi)
        {
            return (posi < AssociatedCards.Count) ? AssociatedCards[posi].assoCard : "";
        }

        public string Icon
        {
            get
            {
                return $"<img src={Url} width=60 t alt='{Count}' title='{Count}'/>";
            }
        }

        public string AssoCard()
        {
            var lst = AssociatedCards.Select(p => $"{p.Icon}").ToList();
            return string.Join("", lst);
        }
        //public void SetImgCards(List<CardBase> cards)
        //{
        //    this.CardImg = cards.FirstOrDefault(c => c.Name == Name).ImgMarkDown;
        //}

        //public void SetImgAssociatedCards(List<CardBase> cards)
        //{
        //    AssociatedCards.ForEach((p) => { p.CardImg = cards.FirstOrDefault(c => c.Name == p.Name).ImgMarkDown; });
        //}
    }

    public class CardBattle
    {
        public CardBattle(BattleLogTeam team, BattleLogTeam opponent)
        {

        }
        public BattleLogTeam BattleLogTeam { get; set; }
    }
}
