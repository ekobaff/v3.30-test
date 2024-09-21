﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BASE_GET_MYINFO_BASIC_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Account.Clan;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_MYINFO_BASIC_ACK : SendPacket
    {
        private Account p;
        private Clan clan;

        public PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Account player)
        {
            p = player;
            clan = ClanManager.getClan(p.clanId);
        }

        public override void write()
        {
            writeH((short)579);
            writeUnicode(p.player_name, 66);
            writeD(p._rank);
            writeD(p._rank);
            writeD(p._gp);
            writeD(p._exp);
            writeD(0);
            writeC(0);
            writeD(0);
            writeD(0);
            writeC(0);
            writeC(0);
            writeD(p._tag);
            writeD(p._money);
            writeD(clan._id);
            writeD(p.clanAccess);
            writeQ(0L);
            writeC((byte)p.pc_cafe);
            writeC((byte)p.tourneyLevel);
            writeUnicode(clan._name, 34);
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
        }
    }
}
