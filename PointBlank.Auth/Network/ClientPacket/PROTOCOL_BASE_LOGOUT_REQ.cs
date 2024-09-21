// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ClientPacket.PROTOCOL_BASE_LOGOUT_REQ
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Network.ServerPacket;
using PointBlank.Core;
using PointBlank.Core.Network;
using System;

namespace PointBlank.Auth.Network.ClientPacket
{
  public class PROTOCOL_BASE_LOGOUT_REQ : ReceivePacket
  {
    public PROTOCOL_BASE_LOGOUT_REQ(AuthClient lc, byte[] buff) => this.makeme(lc, buff);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_LOGOUT_ACK());
        this._client.Close(5000, true);
      }
      catch (Exception ex)
      {
        Logger.warning("PROTOCOL_BASE_LOGOUT_REQ: " + ex.ToString());
      }
    }
  }
}
