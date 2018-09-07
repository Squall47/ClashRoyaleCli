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

    public partial class BattleLogItem
    {
        /// <summary>
        /// Initializes a new instance of the BattleLogItem class.
        /// </summary>
        public BattleLogItem()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the BattleLogItem class.
        /// </summary>
        public BattleLogItem(string type = default(string), string battleTime = default(string), Arena arena = default(Arena), BattleLogItemGameMode gameMode = default(BattleLogItemGameMode), string deckSelection = default(string), IList<BattleLogTeam> team = default(IList<BattleLogTeam>), IList<BattleLogTeam> opponent = default(IList<BattleLogTeam>))
        {
            Type = type;
            BattleTime = battleTime;
            Arena = arena;
            GameMode = gameMode;
            DeckSelection = deckSelection;
            Team = team;
            Opponent = opponent;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "battleTime")]
        public string BattleTime { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "arena")]
        public Arena Arena { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "gameMode")]
        public BattleLogItemGameMode GameMode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "deckSelection")]
        public string DeckSelection { get; set; }

        ///// <summary>
        ///// </summary>
        [JsonProperty(PropertyName = "team")]
        public IList<BattleLogTeam> Team { get; set; }

        ///// <summary>
        ///// </summary>
        [JsonProperty(PropertyName = "opponent")]
        public IList<BattleLogTeam> Opponent { get; set; }

    }
}