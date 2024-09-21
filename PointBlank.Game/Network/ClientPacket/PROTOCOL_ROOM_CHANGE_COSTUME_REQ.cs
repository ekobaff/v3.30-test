﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_ROOM_CHANGE_COSTUME_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_ROOM_CHANGE_COSTUME_REQ : ReceivePacket
  {
    private int Team;

    public PROTOCOL_ROOM_CHANGE_COSTUME_REQ(GameClient Client, byte[] Buffer) => this.makeme(Client, Buffer);

    public override void read() => this.Team = (int) this.readC();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Slot slot = player._room.getSlot(player._slotId);
        if (slot == null && player == null)
          return;
        slot.Costume = this.Team;
        this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_CHANGE_COSTUME_ACK(slot));
      }
      catch (Exception ex)
      {
        Logger.error("PROTOCOL_ROOM_CHANGE_COSTUME_REQ: " + ex.ToString());
      }
    }
  }
}