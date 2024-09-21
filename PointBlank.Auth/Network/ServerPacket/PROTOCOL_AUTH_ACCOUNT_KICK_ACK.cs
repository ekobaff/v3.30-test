// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ServerPacket.PROTOCOL_AUTH_ACCOUNT_KICK_ACK
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Core.Network;

namespace PointBlank.Auth.Network.ServerPacket
{
  public class PROTOCOL_AUTH_ACCOUNT_KICK_ACK : SendPacket
  {
    private int _type;

    public PROTOCOL_AUTH_ACCOUNT_KICK_ACK(int type) => this._type = type;

    public override void write()
    {
      this.writeH((short) 998);
      this.writeC((byte) this._type);
    }
  }
}
