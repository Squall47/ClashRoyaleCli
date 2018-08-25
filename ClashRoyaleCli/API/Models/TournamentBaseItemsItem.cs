// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace ClashRoyale.API.Models
{
    using ClashRoyalCli;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Linq;

    public partial class TournamentBaseItemsItem
    {
        /// <summary>
        /// Initializes a new instance of the TournamentBaseItemsItem class.
        /// </summary>
        public TournamentBaseItemsItem()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the TournamentBaseItemsItem class.
        /// </summary>
        public TournamentBaseItemsItem(string tag = default(string), string type = default(string), string status = default(string), string creatorTag = default(string), string name = default(string), string description = default(string), int? capacity = default(int?), int? maxCapacity = default(int?), int? preparationDuration = default(int?), int? duration = default(int?), DateTime createdTime = default(DateTime))
        {
            Tag = tag;
            Type = type;
            Status = status;
            CreatorTag = creatorTag;
            Name = name;
            Description = description;
            Capacity = capacity;
            MaxCapacity = maxCapacity;
            PreparationDuration = preparationDuration;
            Duration = duration;
            CreatedTime = createdTime;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "creatorTag")]
        public string CreatorTag { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "capacity")]
        public int? Capacity { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "maxCapacity")]
        public int? MaxCapacity { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "preparationDuration")]
        public int? PreparationDuration { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "duration")]
        public int? Duration { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "createdTime")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime CreatedTime { get; set; }

        public override string ToString()
        {
            return $"{CreatedTime.TimeOfDay} : {(MaxCapacity - Capacity).ToString().PadLeft(4)} > {Status.PadRight(14)} = {Name}";
        }

    }
}
