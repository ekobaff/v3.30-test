// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_ROOM_TEAM_BALANCE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_ROOM_TEAM_BALANCE_ACK : SendPacket
  {
    private int _type;
    private int _leader;
    private List<SlotChange> _slots;

    public PROTOCOL_ROOM_TEAM_BALANCE_ACK(List<SlotChange> slots, int leader, int type)
    {
      this._slots = slots;
      this._leader = leader;
      this._type = type;
    }

    public override void write()
    {
      this.writeH((short) 3886);
      this.writeC((byte) this._type);
      this.writeC((byte) this._leader);
      this.writeC((byte) this._slots.Count);
      for (int index = 0; index < this._slots.Count; ++index)
      {
        SlotChange slot = this._slots[index];
        this.writeC((byte) slot.oldSlot._id);
        this.writeC((byte) slot.newSlot._id);
        this.writeC((byte) slot.oldSlot.state);
        this.writeC((byte) slot.newSlot.state);
      }
    }
  }
}
