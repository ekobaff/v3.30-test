// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BASE_DAILY_RECORD_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_BASE_DAILY_RECORD_REQ : ReceivePacket
  {
    public PROTOCOL_BASE_DAILY_RECORD_REQ(GameClient Client, byte[] Buffer) => this.makeme(Client, Buffer);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player.Daily == null)
          return;
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_DAILY_RECORD_ACK(player.Daily));
      }
      catch (Exception ex)
      {
        Logger.error("PROTOCOL_BASE_DAILY_RECORD_REQ: " + ex.ToString());
      }
    }
  }
}
