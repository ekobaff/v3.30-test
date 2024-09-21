// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CS_REQUEST_INFO_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CS_REQUEST_INFO_REQ : ReceivePacket
  {
    private long pId;

    public PROTOCOL_CS_REQUEST_INFO_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.pId = this.readQ();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        this._client.SendPacket((SendPacket) new PROTOCOL_CS_REQUEST_INFO_ACK(this.pId, PlayerManager.getRequestText(player.clanId, this.pId)));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
