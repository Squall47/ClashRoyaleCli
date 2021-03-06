// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace ClashRoyaleApi.API.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class PlayerDetailLeagueStatistics
    {
        /// <summary>
        /// Initializes a new instance of the PlayerDetailLeagueStatistics
        /// class.
        /// </summary>
        public PlayerDetailLeagueStatistics()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the PlayerDetailLeagueStatistics
        /// class.
        /// </summary>
        public PlayerDetailLeagueStatistics(SeasonStatistics currentSeason = default(SeasonStatistics), SeasonStatistics previousSeason = default(SeasonStatistics), SeasonStatistics bestSeason = default(SeasonStatistics))
        {
            CurrentSeason = currentSeason;
            PreviousSeason = previousSeason;
            BestSeason = bestSeason;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "currentSeason")]
        public SeasonStatistics CurrentSeason { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "previousSeason")]
        public SeasonStatistics PreviousSeason { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "bestSeason")]
        public SeasonStatistics BestSeason { get; set; }

    }
}
