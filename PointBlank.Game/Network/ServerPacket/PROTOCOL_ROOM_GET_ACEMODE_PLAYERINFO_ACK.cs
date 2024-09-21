using System;
using PointBlank.Core.Models.Account.Clan;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
    // Token: 0x020000CE RID: 206
    public class PROTOCOL_ROOM_GET_ACEMODE_PLAYERINFO_ACK : SendPacket
    {
        // Token: 0x060001E5 RID: 485 RVA: 0x00003B4A File Offset: 0x00001D4A
        public PROTOCOL_ROOM_GET_ACEMODE_PLAYERINFO_ACK(Account p)
        {
            this.Player = p;
            this.clan = ClanManager.getClan(p.clanId);
        }

        // Token: 0x060001E6 RID: 486 RVA: 0x0000FCA0 File Offset: 0x0000DEA0
        public override void write()
        {
            ClanManager.getClan(this.Player.clanId);
            base.writeH(3935);
            base.writeH(0);
            base.writeD(this.Player.LastRankUpDate);
            base.writeD(this.Player._statistic.fights);
            base.writeD(this.Player._statistic.fights_win);
            base.writeD(this.Player._statistic.fights_win);
            base.writeD(this.Player._statistic.kills_count);
            base.writeD(this.Player._statistic.deaths_count);
            base.writeD(this.Player._statistic.headshots_count);
            base.writeD(this.Player._statistic.totalfights_count);
            base.writeD(this.Player._statistic.totalkills_count);
            base.writeD(this.Player._statistic.escapes);
            base.writeD(this.Player._statistic.assist);
            base.writeD(this.Player._statistic.fights);
            base.writeD(this.Player._statistic.fights_win);
            base.writeD(this.Player._statistic.fights_win);
            base.writeD(this.Player._statistic.kills_count);
            base.writeD(this.Player._statistic.deaths_count);
            base.writeD(this.Player._statistic.headshots_count);
            base.writeD(this.Player._statistic.totalfights_count);
            base.writeD(this.Player._statistic.totalkills_count);
            base.writeD(this.Player._statistic.escapes);
            base.writeD(this.Player._statistic.assist);
            base.writeB(new byte[255]);
            base.writeUnicode(this.Player.player_name, 66);
            base.writeD(this.Player._rank);
            base.writeD(this.Player._rank);
            base.writeD(this.Player._gp);
            base.writeD(this.Player._exp);
            base.writeD(0);
            base.writeC(0);
            base.writeD(0);
            base.writeQ(0L);
            base.writeC(0);
            base.writeC(0);
            base.writeD(this.Player._money);
            base.writeD(this.clan._id);
            base.writeD(this.Player.clanAccess);
            base.writeQ(0L);
            base.writeC((byte)this.Player.pc_cafe);
            base.writeC((byte)this.Player.tourneyLevel);
            base.writeUnicode(this.clan._name, 34);
            base.writeC((byte)this.clan._rank);
            base.writeC((byte)this.clan.getClanUnit());
            base.writeD(this.clan._logo);
            base.writeC((byte)this.clan._name_color);
            base.writeC((byte)this.clan.effect);
            base.writeC((byte)this.Player.age);
        }

        // Token: 0x04000167 RID: 359
        public Account Player;

        // Token: 0x04000168 RID: 360
        private Clan clan;
    }
}
