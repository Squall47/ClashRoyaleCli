// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace ClashRoyaleApi.API.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class PlayerRankingList
    {
        /// <summary>
        /// Initializes a new instance of the PlayerRankingList class.
        /// </summary>
        public PlayerRankingList()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the PlayerRankingList class.
        /// </summary>
        public PlayerRankingList(IList<PlayerRanking> items = default(IList<PlayerRanking>))
        {
            Items = items;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public IList<PlayerRanking> Items { get; set; }

    }
}
