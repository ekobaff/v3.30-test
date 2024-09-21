// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BATTLE_ENDBATTLE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Account.Clan;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Utils;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_ENDBATTLE_ACK : SendPacket
    {
        private Room r;
        private Account p;
        private int Winner = 2;
        private ushort PlayersFlag;
        private ushort MissionsFlag;
        private bool BotMode;

        public PROTOCOL_BATTLE_ENDBATTLE_ACK(Account p)
        {
            this.p = p;
            if (p == null)
                return;
            r = p._room;
            Winner = r.room_type != RoomType.Tutorial ? (int)AllUtils.GetWinnerTeam(r) : 0;
            BotMode = r.isBotMode();
            AllUtils.getBattleResult(r, out MissionsFlag, out PlayersFlag);
        }

        public PROTOCOL_BATTLE_ENDBATTLE_ACK(Account p, int Winner,  ushort PlayersFlag, ushort MissionsFlag,  bool BotMode)
        {
            this.p = p;
            this.Winner = Winner;
            this.PlayersFlag = PlayersFlag;
            this.MissionsFlag = MissionsFlag;
            this.BotMode = BotMode;
            if (p == null)
                return;
            r = p._room;
        }

        public PROTOCOL_BATTLE_ENDBATTLE_ACK(Account p, TeamResultType Winner, ushort PlayersFlag, ushort MissionsFlag,  bool BotMode)
        {
            this.p = p;
            this.Winner = (int)Winner;
            this.PlayersFlag = PlayersFlag;
            this.MissionsFlag = MissionsFlag;
            this.BotMode = BotMode;
            if (p == null)
                return;
            r = p._room;
        }

        public override void write()
        {
            if (p == null || r == null)
                return;
            Clan clan = ClanManager.getClan(p.clanId);
            writeH((short)4116);
            writeH(PlayersFlag);
            writeC((byte)Winner);
            for (int index = 0; index < 16; ++index)
                writeH((ushort)r._slots[index].exp);
            for (int index = 0; index < 16; ++index)
                writeH((ushort)r._slots[index].gp);
            for (int index = 0; index < 16; ++index)
                writeC((byte)r._slots[index].bonusFlags);
            for (int index = 0; index < 16; ++index)
            {
                Slot slot = r._slots[index];
                writeH((ushort)slot.BonusCafeExp);
                writeH((ushort)slot.BonusItemExp);
                writeH((ushort)slot.BonusEventExp);
            }
            for (int index = 0; index < 16; ++index)
            {
                Slot slot = r._slots[index];
                writeH((ushort)slot.BonusCafePoint);
                writeH((ushort)slot.BonusItemPoint);
                writeH((ushort)slot.BonusEventPoint);
            }
            writeH(MissionsFlag);
            if (BotMode)
            {
                for (int index = 0; index < 16; ++index)
                    writeH((ushort)r._slots[index].Score);
            }
            else if (r.room_type == RoomType.Bomb || r.room_type == RoomType.Annihilation || r.room_type == RoomType.Boss || r.room_type == RoomType.CrossCounter || r.room_type == RoomType.Convoy || r.room_type == RoomType.Defense || r.room_type == RoomType.Destroy)
            {
                writeH(r.room_type == RoomType.Boss ? (ushort)r.red_dino : (r.room_type == RoomType.CrossCounter ? (ushort)r._redKills : (ushort)r.red_rounds));
                writeH(r.room_type == RoomType.Boss ? (ushort)r.blue_dino : (r.room_type == RoomType.CrossCounter ? (ushort)r._blueKills : (ushort)r.blue_rounds));
                for (int index = 0; index < 16; ++index)
                    writeC((byte)r._slots[index].objects);
            }
            writeC((byte)0);
            writeC((byte)0);
            writeC((byte)0);
            if (BotMode || r.SlotRewards is null)
                writeB(new byte[25]
            {
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue
            });
            else
            {
                writeB(r.SlotRewards);
                writeD(r.ItensRewards[0]);
                writeD(r.ItensRewards[1]);
                writeD(r.ItensRewards[2]);
                writeD(r.ItensRewards[3]);
                writeD(r.ItensRewards[4]);
            }
            writeC((byte)(p.player_name.Length * 2));
            writeUnicode(p.player_name, p.player_name.Length * 2);
            writeD(p.getRank());
            writeD(p.getRank());
            writeD(p._gp);
            writeD(p._exp);
            writeD(0);
            writeC(0);
            writeD(0);
            writeD(0);
            writeC(0);
            writeC(0);
            writeD(p._tag); //tag
            writeD(p._money);
            writeD(clan._id);
            writeD(p.clanAccess);
            writeQ(0L);
            writeC((byte)p.pc_cafe);
            writeC((byte)p.tourneyLevel);
            writeC((byte)(clan._name.Length * 2));
            writeUnicode(clan._name, clan._name.Length * 2);
            writeC((byte)clan._rank);
            writeC((byte)clan.getClanUnit());
            writeD(clan._logo);
            writeC((byte)clan._name_color);
            writeC((byte)clan.effect);
            writeD(p._statistic.fights);
            writeD(p._statistic.fights_win);
            writeD(p._statistic.fights_lost);
            writeD(p._statistic.fights_draw);
            writeD(p._statistic.kills_count);
            writeD(p._statistic.headshots_count);
            writeD(p._statistic.deaths_count);
            writeD(p._statistic.totalfights_count);
            writeD(p._statistic.totalkills_count);
            writeD(p._statistic.escapes);
            writeD(p._statistic.assist);
            writeD(p._statistic.fights);
            writeD(p._statistic.fights_win);
            writeD(p._statistic.fights_lost);
            writeD(p._statistic.fights_draw);
            writeD(p._statistic.kills_count);
            writeD(p._statistic.headshots_count);
            writeD(p._statistic.deaths_count);
            writeD(p._statistic.totalfights_count);
            writeD(p._statistic.totalkills_count);
            writeD(p._statistic.escapes);
            writeD(p._statistic.assist);
            writeH((short)p.Daily.Total);
            writeH((short)p.Daily.Wins);
            writeH((short)p.Daily.Loses);
            writeH((short)p.Daily.Draws);
            writeH((short)p.Daily.Kills);
            writeH((short)p.Daily.Headshots);
            writeH((short)p.Daily.Deaths);
            writeD(p.Daily.Exp);
            writeD(p.Daily.Point);
            writeB(new byte[16]);
            writeC((byte)0);
            writeD(0);
            writeD(0);
        }
    }
}
