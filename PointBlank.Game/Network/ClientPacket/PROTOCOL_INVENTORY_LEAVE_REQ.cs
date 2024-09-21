﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_INVENTORY_LEAVE_REQ
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
  public class PROTOCOL_INVENTORY_LEAVE_REQ : ReceivePacket
  {
    public PROTOCOL_INVENTORY_LEAVE_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        Account player = this._client._player;
        if (player == null)
          return;
        player._room?.changeSlotState(player._slotId, SlotState.NORMAL, true);
        this._client.SendPacket((SendPacket) new PROTOCOL_INVENTORY_LEAVE_ACK(0));
      }
      catch (Exception ex)
      {
        Logger.info("PROTOCOL_INVENTORY_LEAVE_REQ: " + ex.ToString());
      }
    }
  }
}