// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_ROOM_JOIN_REQ
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
  public class PROTOCOL_ROOM_JOIN_REQ : ReceivePacket
  {
    private int roomId;
    private int type;
    private string password;

    public PROTOCOL_ROOM_JOIN_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.roomId = this.readD();
      this.password = this.readS(4);
      this.type = (int) this.readC2();
      int num = (int) this.readC();
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Channel channel;
        if (player != null && player.player_name.Length > 0 && player._room == null && player._match == null && player.getChannel(out channel))
        {
          Room room = channel.getRoom(this.roomId);
          Account p;
          if (room != null && room.getLeader(out p))
          {
            if (room.room_type == RoomType.Tutorial)
              this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_JOIN_ACK(2147487868U));
            else if (room.password.Length > 0 && this.password != room.password && player._rank != 53 && !player.HaveGMLevel() && this.type != 1)
              this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_JOIN_ACK(2147487749U));
            else if (room.Limit == (byte) 1 && room._state >= RoomState.CountDown && !player.HaveGMLevel())
              this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_JOIN_ACK(2147487763U));
            else if (room.kickedPlayers.Contains(player.player_id) && !player.HaveGMLevel())
              this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_JOIN_ACK(2147487756U));
            else if (room.addPlayer(player) >= 0)
            {
              player.ResetPages();
              using (PROTOCOL_ROOM_GET_SLOTONEINFO_ACK getSlotoneinfoAck = new PROTOCOL_ROOM_GET_SLOTONEINFO_ACK(player))
                room.SendPacketToPlayers((SendPacket) getSlotoneinfoAck, player.player_id);
              this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_JOIN_ACK(0U, player, p));
               WarningRanked(player, room);
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
        Logger.warning("PROTOCOL_LOBBY_JOIN_ROOM_REQ: " + ex.ToString());
      }
    }

    public void WarningRanked(Account p, Room r)    
    {

            if (r.name.ToLower().Contains("@ranked") && r.room_type == RoomType.Bomb && GameManager.Config.RankedEnable) // Mexido aqui lembra amanha
                p.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("Você está prestes a jogar modo ranqueado.\nranqueado é um modo competitivo que permite jogar, e ganha pontos para se manter na classificação, Durante as Temporadas ranqueado, você também pode ganhar recompensas de acordo com sua classificação"));
    }
    }
}
