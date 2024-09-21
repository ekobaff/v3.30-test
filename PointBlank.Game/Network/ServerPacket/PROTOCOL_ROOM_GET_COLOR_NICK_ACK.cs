// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_ROOM_GET_COLOR_NICK_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_ROOM_GET_COLOR_NICK_ACK : SendPacket
  {
    private int Slot;
    private int Color;

    public PROTOCOL_ROOM_GET_COLOR_NICK_ACK(int Slot, int Color)
    {
      this.Slot = Slot;
      this.Color = Color;
    }

    public override void write()
    {
      this.writeH((short) 3892);
      this.writeD(this.Slot);
      this.writeC((byte) this.Color);
    }
  }
}
