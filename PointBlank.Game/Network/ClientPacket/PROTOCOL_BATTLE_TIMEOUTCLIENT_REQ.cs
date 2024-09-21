// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BATTLE_TIMEOUTCLIENT_REQ
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
  public class PROTOCOL_BATTLE_TIMEOUTCLIENT_REQ : ReceivePacket
  {
    private int Slot;

    public PROTOCOL_BATTLE_TIMEOUTCLIENT_REQ(GameClient Client, byte[] Buffer) => this.makeme(Client, Buffer);

    public override void read() => this.Slot = this.readD();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Room room = player._room;
        if (player == null || room == null || player._slotId != this.Slot)
          return;
        player._connection.SendPacket((SendPacket) new PROTOCOL_BATTLE_TIMEOUTCLIENT_ACK());
      }
      catch (Exception ex)
      {
        Logger.error("PROTOCOL_BATTLE_TIMEOUTCLIENT_REQ: " + ex.ToString());
      }
    }
  }
}
