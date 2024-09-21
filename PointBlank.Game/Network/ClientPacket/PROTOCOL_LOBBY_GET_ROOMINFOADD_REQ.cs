﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_LOBBY_GET_ROOMINFOADD_REQ
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
  public class PROTOCOL_LOBBY_GET_ROOMINFOADD_REQ : ReceivePacket
  {
    private int roomId;

    public PROTOCOL_LOBBY_GET_ROOMINFOADD_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.roomId = this.readD();

    public override void run()
    {
      if (this._client == null)
        return;
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Channel channel = player.getChannel();
        if (channel == null)
          return;
        Room room = channel.getRoom(this.roomId);
        Account p;
        if (room == null || !room.getLeader(out p))
          return;
        this._client.SendPacket((SendPacket) new PROTOCOL_LOBBY_GET_ROOMINFOADD_ACK(room, p));
      }
      catch (Exception ex)
      {
        Logger.warning("PROTOCOL_LOBBY_GET_ROOMINFOADD_REQ: " + ex.ToString());
      }
    }
  }
}
