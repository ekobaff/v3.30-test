// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CLAN_WAR_LEAVE_TEAM_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CLAN_WAR_LEAVE_TEAM_REQ : ReceivePacket
  {
    private uint erro;

    public PROTOCOL_CLAN_WAR_LEAVE_TEAM_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Match match = player == null ? (Match) null : player._match;
        if (match == null || !match.RemovePlayer(player))
          this.erro = 2147483648U;
        this._client.SendPacket((SendPacket) new PROTOCOL_CLAN_WAR_LEAVE_TEAM_ACK(this.erro));
        if (this.erro != 0U)
          return;
        player._status.updateClanMatch(byte.MaxValue);
        AllUtils.syncPlayerToClanMembers(player);
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
