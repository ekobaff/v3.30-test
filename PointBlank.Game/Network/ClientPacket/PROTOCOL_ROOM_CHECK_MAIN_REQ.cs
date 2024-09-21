// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_ROOM_CHECK_MAIN_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ClientPacket
{
  internal class PROTOCOL_ROOM_CHECK_MAIN_REQ : ReceivePacket
  {
    private List<PointBlank.Core.Models.Room.Slot> slots = new List<PointBlank.Core.Models.Room.Slot>();
    private uint erro;

    public PROTOCOL_ROOM_CHECK_MAIN_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        PointBlank.Game.Data.Model.Room room = player == null ? (PointBlank.Game.Data.Model.Room) null : player._room;
        if (room != null && room._leader == player._slotId && room._state == RoomState.Ready)
        {
          lock (room._slots)
          {
            for (int index = 0; index < 16; ++index)
            {
              PointBlank.Core.Models.Room.Slot slot = room._slots[index];
              if (slot._playerId > 0L && index != room._leader)
                this.slots.Add(slot);
            }
          }
          if (this.slots.Count > 0)
          {
            PointBlank.Core.Models.Room.Slot slot = this.slots[new Random().Next(this.slots.Count)];
            this.erro = room.getPlayerBySlot(slot) != null ? (uint) slot._id : 2147483648U;
            this.slots = (List<PointBlank.Core.Models.Room.Slot>) null;
          }
          else
            this.erro = 2147483648U;
        }
        else
          this.erro = 2147483648U;
        this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_CHECK_MAIN_ACK(this.erro));
      }
      catch (Exception ex)
      {
        Logger.info("PROTOCOL_ROOM_CHECK_MAIN_REQ: " + ex.ToString());
      }
    }
  }
}
