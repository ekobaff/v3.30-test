// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ServerPacket.PROTOCOL_SERVER_MESSAGE_ERROR_ACK
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Core.Network;

namespace PointBlank.Auth.Network.ServerPacket
{
  public class PROTOCOL_SERVER_MESSAGE_ERROR_ACK : SendPacket
  {
    private uint _er;

    public PROTOCOL_SERVER_MESSAGE_ERROR_ACK(uint er) => this._er = er;

    public override void write()
    {
      this.writeH((short) 2566);
      this.writeD(this._er);
    }
  }
}
