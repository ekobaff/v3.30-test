// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CS_MEMBER_LIST_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CS_MEMBER_LIST_REQ : ReceivePacket
  {
    private int page;

    public PROTOCOL_CS_MEMBER_LIST_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.page = (int) this.readC();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        if (ClanManager.getClan(player.FindClanId)._id == 0)
        {
          this._client.SendPacket((SendPacket) new PROTOCOL_CS_MEMBER_LIST_ACK(-1));
        }
        else
        {
          List<Account> clanPlayers = ClanManager.getClanPlayers(player.FindClanId, -1L, false);
          using (SendGPacket p = new SendGPacket())
          {
            int count = 0;
            for (int index = this.page * 14; index < clanPlayers.Count; ++index)
            {
              this.WriteData(clanPlayers[index], p);
              if (++count == 14)
                break;
            }
            this._client.SendPacket((SendPacket) new PROTOCOL_CS_MEMBER_LIST_ACK(this.page, count, p.mstream.ToArray()));
          }
        }
      }
      catch (Exception ex)
      {
        Logger.error("PROTOCOL_CS_MEMBER_LIST_REQ[" + this._client.player_id.ToString() + "]: " + ex.ToString());
      }
    }

    private void WriteData(Account member, SendGPacket p)
    {
      p.writeQ(member.player_id);
      p.writeUnicode(member.player_name, 66);
      p.writeC((byte) member._rank);
      p.writeC((byte) member.clanAccess);
      p.writeQ(ComDiv.GetClanStatus(member._status, member._isOnline));
      p.writeD(member.clanDate);
      p.writeC((byte) member.name_color);
      p.writeD(member._statistic.ClanGames);
      p.writeD(member._statistic.ClanWins);
    }
  }
}
