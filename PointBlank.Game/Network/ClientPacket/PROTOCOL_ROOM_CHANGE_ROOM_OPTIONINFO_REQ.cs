﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_REQ
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
  public class PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_REQ : ReceivePacket
  {
    private string leader;

    public PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      try
      {
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        if (room == null || room._leader != player._slotId || room._state != RoomState.Ready)
          return;
        this.leader = this.readS(33);
        room.killtime = this.readD();
        room.Limit = this.readC();
        room.WatchRuleFlag = this.readC();
        room.BalanceType = this.readH();
        using (PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_ACK roomOptioninfoAck = new PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_ACK(room, this.leader))
          room.SendPacketToPlayers((SendPacket) roomOptioninfoAck);
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }

    public override void run()
    {
    }
  }
}
