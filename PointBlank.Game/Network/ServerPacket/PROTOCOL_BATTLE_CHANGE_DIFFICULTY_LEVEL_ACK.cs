// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_ACK : SendPacket
  {
    private Room room;

    public PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_ACK(Room room) => this.room = room;

    public override void write()
    {
      this.writeH((short) 4149);
      this.writeC(this.room.IngameAiLevel);
      for (int index = 0; index < 16; ++index)
        this.writeD(this.room._slots[index].aiLevel);
    }
  }
}
