// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ServerPacket.PROTOCOL_BASE_URL_LIST_ACK
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Core.Network;

namespace PointBlank.Auth.Network.ServerPacket
{
  public class PROTOCOL_BASE_URL_LIST_ACK : SendPacket
  {
    public override void write()
    {
      string name1 = "";
      string name2 = "";
      this.writeH((short) 673);
      this.writeC((byte) 1);
      this.writeC((byte) 2);
      this.writeH((ushort) name1.Length);
      this.writeQ(4L);
      this.writeText(name1, name1.Length);
      this.writeH((ushort) name2.Length);
      this.writeQ(3L);
      this.writeText(name2, name2.Length);
    }
  }
}
