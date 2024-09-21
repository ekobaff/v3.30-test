// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BASE_GAMEGUARD_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_BASE_GAMEGUARD_REQ : ReceivePacket
  {
    private byte[] Bytes;
    private string ClientVersion;

    public PROTOCOL_BASE_GAMEGUARD_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.Bytes = this.readB(48);
      this.ClientVersion = this.readC().ToString() + "." + this.readH().ToString();
    }

    public override void run()
    {
      try
      {
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_GAMEGUARD_ACK());
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
