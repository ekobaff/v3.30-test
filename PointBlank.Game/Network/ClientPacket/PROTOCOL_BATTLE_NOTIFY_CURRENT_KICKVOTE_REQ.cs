// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Threading;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_REQ : ReceivePacket
  {
    private byte type;

    public PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.type = this.readC();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        PointBlank.Game.Data.Model.Room room = player == null ? (PointBlank.Game.Data.Model.Room) null : player._room;
        Slot slot;
        if (room == null || room._state != RoomState.Battle || room.vote.Timer == null || room.votekick == null || !room.getSlot(player._slotId, out slot) || slot.state != SlotState.BATTLE)
          return;
        PointBlank.Core.Models.Room.VoteKick votekick = room.votekick;
        if (votekick._votes.Contains(player._slotId))
        {
          this._client.SendPacket((SendPacket) new PROTOCOL_BATTLE_VOTE_KICKVOTE_ACK(2147487985U));
        }
        else
        {
          lock (votekick._votes)
            votekick._votes.Add(slot._id);
          if (this.type == (byte) 0)
          {
            ++votekick.kikar;
            if (slot._team == votekick.victimIdx % 2)
              ++votekick.allies;
            else
              ++votekick.enemys;
          }
          else
            ++votekick.deixar;
          if (votekick._votes.Count >= votekick.GetInGamePlayers())
          {
            room.vote.Timer = (Timer) null;
            AllUtils.votekickResult(room);
          }
          else
          {
            using (PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_ACK currentKickvoteAck = new PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_ACK(votekick))
              room.SendPacketToPlayers((SendPacket) currentKickvoteAck, SlotState.BATTLE, 0);
          }
        }
      }
      catch (Exception ex)
      {
        Logger.info("PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_REQ: " + ex.ToString());
      }
    }
  }
}
