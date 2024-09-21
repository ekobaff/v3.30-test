// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BATTLE_ACE_MODE_SLOT_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Game.Network.ServerPacket;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ClientPacket
{
  internal class PROTOCOL_BATTLE_ACE_MODE_SLOT_REQ : ReceivePacket
  {
    private int SlotId;

    public PROTOCOL_BATTLE_ACE_MODE_SLOT_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.SlotId = (int) this.readC();

    public override void run()
    {
      if (this._client == null || this._client._player == null || this._client._player._room == null)
        return;
      PointBlank.Game.Data.Model.Room room = this._client._player._room;
      if (this._client._player._slotId == this.SlotId)
        return;
      PointBlank.Core.Models.Room.Slot slot1 = room.getSlot(this._client._player._slotId);
      PointBlank.Core.Models.Room.Slot slot2 = room.getSlot(this.SlotId);
      if (slot2._playerId > 0L || slot2.state != SlotState.EMPTY)
        return;
      slot2.state = SlotState.NORMAL;
      slot2._playerId = this._client._player.player_id;
      slot2._equip = this._client._player._equip;
      slot1.state = SlotState.EMPTY;
      slot1._playerId = 0L;
      slot1._equip = (PlayerEquipedItems) null;
      List<SlotChange> slots = new List<SlotChange>();
      slots.Add(new SlotChange()
      {
        oldSlot = slot1,
        newSlot = slot2
      });
      if (this._client._player._slotId == room._leader)
        room._leader = this.SlotId;
      this._client._player._slotId = this.SlotId;
      using (PROTOCOL_ROOM_TEAM_BALANCE_ACK roomTeamBalanceAck = new PROTOCOL_ROOM_TEAM_BALANCE_ACK(slots, room._leader, 0))
        room.SendPacketToPlayers((SendPacket) roomTeamBalanceAck);
    }
  }
}
