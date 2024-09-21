// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ClientPacket.PROTOCOL_BASE_GAMEGUARD_REQ
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Network.ServerPacket;
using PointBlank.Core.Network;

namespace PointBlank.Auth.Network.ClientPacket
{
  public class PROTOCOL_BASE_GAMEGUARD_REQ : ReceivePacket
  {
    private string ClientVersion;

    public PROTOCOL_BASE_GAMEGUARD_REQ(AuthClient Client, byte[] Buffer) => this.makeme(Client, Buffer);

    public override void read()
    {
      this.readB(48);
      this.ClientVersion = this.readC().ToString() + "." + this.readH().ToString();
    }

    public override void run() => this._client.SendPacket((SendPacket) new PROTOCOL_BASE_GAMEGUARD_ACK());
  }
}
