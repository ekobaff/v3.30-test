// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CS_ROOM_INVITED_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CS_ROOM_INVITED_REQ : ReceivePacket
  {
    private long pId;

    public PROTOCOL_CS_ROOM_INVITED_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.pId = this.readQ();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || player.clanId == 0)
          return;
        Account account = AccountManager.getAccount(this.pId, 0);
        if (account != null && account.clanId == player.clanId)
          account.SendPacket((SendPacket) new PROTOCOL_CS_ROOM_INVITED_RESULT_ACK(this._client.player_id), false);
        player.SendPacket((SendPacket) new PROTOCOL_CS_ROOM_INVITED_ACK(0));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
