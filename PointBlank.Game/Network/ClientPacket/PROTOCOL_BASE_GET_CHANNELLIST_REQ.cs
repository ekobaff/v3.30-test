// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BASE_GET_CHANNELLIST_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Xml;
using PointBlank.Game.Network.ServerPacket;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_BASE_GET_CHANNELLIST_REQ : ReceivePacket
  {
    private int ServerId;

    public PROTOCOL_BASE_GET_CHANNELLIST_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.ServerId = this.readD();

    public override void run()
    {
      List<Channel> channels = ChannelsXml.getChannels(this.ServerId);
      if (this._client._player == null)
        return;
      this._client.SendPacket((SendPacket) new PROTOCOL_BASE_GET_CHANNELLIST_ACK(channels));
    }
  }
}
