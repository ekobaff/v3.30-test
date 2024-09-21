// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BASE_CHATTING_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BASE_CHATTING_ACK : SendPacket
  {
    private int erro;
    private int banTime;

    public PROTOCOL_BASE_CHATTING_ACK(int erro, int time = 0)
    {
      this.erro = erro;
      this.banTime = time;
    }

    public override void write()
    {
      this.writeH((short) 2628);
      this.writeC((byte) this.erro);
      if (this.erro <= 0)
        return;
      this.writeD(this.banTime);
    }
  }
}
