namespace ClashRoyalCli.APIExtend.Models
{
    public class CardUsage
    {
        public string Name { get; set; }
        public int UsageCount { get; set; }
        public override string ToString()
        {
            return $"{UsageCount.ToString().PadLeft(4)} = {Name}";
        }
    }
}
