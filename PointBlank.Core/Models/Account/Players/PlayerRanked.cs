using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Core.Models.Account.Players
{
    public class PlayerRanked
    {
        public long PlayerId;
        public int Rank;
        public int Exp;
        public int Matches;
        public int Wins;
        public int Loses;
        public int Kills;
        public int Deaths;
        public int Headshots;
        public int Playtime;
    }
}
