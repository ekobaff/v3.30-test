// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_ROOM_INFO_ENTER_REQ
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
  public class PROTOCOL_ROOM_INFO_ENTER_REQ : ReceivePacket
  {
    public PROTOCOL_ROOM_INFO_ENTER_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        if (room != null)
        {
          room.changeSlotState(player._slotId, SlotState.INFO, false);
          room.StopCountDown(player._slotId);
          room.updateSlotsInfo();
        }
        this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_INFO_ENTER_ACK());
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
      }
    }
  }
}
