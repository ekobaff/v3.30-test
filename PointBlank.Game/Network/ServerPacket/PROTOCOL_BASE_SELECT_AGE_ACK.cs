// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BASE_SELECT_AGE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BASE_SELECT_AGE_ACK : SendPacket
  {
    private int Error;
    private int Age;

    public PROTOCOL_BASE_SELECT_AGE_ACK(int Error, int Age)
    {
      this.Error = Error;
      this.Age = Age;
    }

    public override void write()
    {
      this.writeH((short) 3096);
      this.writeD(this.Error);
      if (this.Error != 0)
        return;
      this.writeC((byte) this.Age);
    }
  }
}
