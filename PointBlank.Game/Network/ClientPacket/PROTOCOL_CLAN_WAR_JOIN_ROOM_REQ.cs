﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CLAN_WAR_JOIN_ROOM_REQ
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
  public class PROTOCOL_CLAN_WAR_JOIN_ROOM_REQ : ReceivePacket
  {
    private int match;
    private int channel;
    private int unk;

    public PROTOCOL_CLAN_WAR_JOIN_ROOM_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.match = this.readD();
      this.unk = (int) this.readH();
      this.channel = (int) this.readH();
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || player.clanId == 0 || player._match == null)
          return;
        Channel channel;
        if (player != null && player.player_name.Length > 0 && player._room == null && player.getChannel(out channel))
        {
          Room room = channel.getRoom(this.match);
          Account p;
          if (room != null && room.getLeader(out p))
          {
            if (room.password.Length > 0 && !player.HaveGMLevel())
              this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_JOIN_ACK(2147487749U));
            else if (room.Limit == (byte) 1 && room._state >= RoomState.CountDown)
              this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_JOIN_ACK(2147487763U));
            else if (room.kickedPlayers.Contains(player.player_id))
              this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_JOIN_ACK(2147487756U));
            else if (room.addPlayer(player, this.unk) >= 0)
            {
              using (PROTOCOL_ROOM_GET_SLOTONEINFO_ACK getSlotoneinfoAck = new PROTOCOL_ROOM_GET_SLOTONEINFO_ACK(player))
                room.SendPacketToPlayers((SendPacket) getSlotoneinfoAck, player.player_id);
              this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_JOIN_ACK(0U, player, p));
            }
            else
              this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_JOIN_ACK(2147487747U));
          }
          else
            this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_JOIN_ACK(2147487748U));
        }
        else
          this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_JOIN_ACK(2147487748U));
      }
      catch (Exception ex)
      {
        Logger.warning("PROTOCOL_CLAN_WAR_JOIN_ROOM_REQ: " + ex.ToString());
      }
    }
  }
}
