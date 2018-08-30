namespace ClashRoyale.API.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class BattleLogItem
    {

        public bool IsSame(BattleLogItem item)
        {
            var team = this.Team.First();
            var oppo = this.Opponent.First();
            var itemteam = item.Team.First();
            var itemoppo = item.Opponent.First();
            return team.Tag == itemoppo.Tag && oppo.Tag == itemteam.Tag &&
                team.Crowns == itemoppo.Crowns && oppo.Crowns == itemteam.Crowns;
        }

        private IList<BattleLogTeam> _winners;
        public IList<BattleLogTeam> Winners
        {
            get
            {
                InitWinners();
                return _winners;
            }
        }

        public void InitWinners()
        {
            if (_winners == null)
            {
                if (Team != null && Opponent != null)
                {
                    if (Team.First().Crowns > Opponent.First().Crowns)
                    {
                        _winners = Team;
                    }
                    else if (Team.First().Crowns < Opponent.First().Crowns)
                    {
                        _winners = Opponent;
                    }
                    else
                    {
                        _winners = new List<BattleLogTeam>();
                    }
                }
            }
        }
    }
}