// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace ClashRoyaleApi.API.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class SeasonStatistics
    {
        /// <summary>
        /// Initializes a new instance of the SeasonStatistics class.
        /// </summary>
        public SeasonStatistics()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the SeasonStatistics class.
        /// </summary>
        public SeasonStatistics(string id = default(string), int? trophies = default(int?), int? bestTrophies = default(int?))
        {
            Id = id;
            Trophies = trophies;
            BestTrophies = bestTrophies;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "trophies")]
        public int? Trophies { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "bestTrophies")]
        public int? BestTrophies { get; set; }

    }
}