// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BATTLE_MISSION_TOUCHDOWN_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BATTLE_MISSION_TOUCHDOWN_ACK : SendPacket
  {
    private PointBlank.Game.Data.Model.Room r;
    private Slot slot;

    public PROTOCOL_BATTLE_MISSION_TOUCHDOWN_ACK(PointBlank.Game.Data.Model.Room room, Slot slot)
    {
      this.r = room;
      this.slot = slot;
    }

    public override void write()
    {
      this.writeH((short) 4155);
      this.writeH((ushort) this.r.red_dino);
      this.writeH((ushort) this.r.blue_dino);
      this.writeD(this.slot._id);
      this.writeH((short) this.slot.passSequence);
    }
  }
}
