// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ServerPacket.PROTOCOL_BASE_USER_LEAVE_ACK
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Core.Network;

namespace PointBlank.Auth.Network.ServerPacket
{
  public class PROTOCOL_BASE_USER_LEAVE_ACK : SendPacket
  {
    private int erro;

    public PROTOCOL_BASE_USER_LEAVE_ACK(int erro) => this.erro = erro;

    public override void write()
    {
      this.writeH((short) 537);
      this.writeH((short) 0);
      this.writeD(this.erro);
    }
  }
}
