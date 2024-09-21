// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_REQ : ReceivePacket
  {
    private int slotId;

    public PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.slotId = this.readD();

    public override void run()
    {
      Account player = this._client._player;
      Room room = player == null ? (Room) null : player._room;
      try
      {
        if (room == null || room._leader == this.slotId || room._slots[this.slotId]._playerId == 0L)
        {
          this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK(2147483648U));
        }
        else
        {
          if (room._state != RoomState.Ready || room._leader != player._slotId)
            return;
          room.setNewLeader(this.slotId, 0, room._leader, false);
          using (PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK mainChangeWhoAck = new PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK(this.slotId))
            room.SendPacketToPlayers((SendPacket) mainChangeWhoAck);
          room.updateSlotsInfo();
        }
      }
      catch (Exception ex)
      {
        Logger.info("PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_REQ: " + ex.ToString());
      }
    }
  }
}
