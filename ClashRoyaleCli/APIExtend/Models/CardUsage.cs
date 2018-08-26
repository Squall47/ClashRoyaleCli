using System.Collections.Generic;
using System.Linq;

namespace ClashRoyalCli.APIExtend.Models
{
    public class CardUsage
    {
        public string Name { get; set; }
        public int UsageCount { get; set; }
        public IList<CardUsage> AssociatedCards { get; set; }

        public override string ToString()
        {
            var asso = AssociatedCards != null ? " > " + string.Join(",", AssociatedCards) : "";
            return $"{UsageCount.ToString().PadLeft(4)}={Name}{asso}";
        }
    }
}
