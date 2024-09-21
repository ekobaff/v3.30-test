// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Xml;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_REQ : ReceivePacket
  {
    private int id;
    private int serverInfo;

    public PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.id = (int) this.readH();
      this.serverInfo = (int) this.readH();
    }

    public override void run()
    {
      Account player = this._client._player;
      if (player == null)
        return;
      if (player._match == null)
        return;
      try
      {
        Channel channel = ChannelsXml.getChannel(this.serverInfo - this.serverInfo / 10 * 10);
        if (channel != null)
        {
          Match match = channel.getMatch(this.id);
          if (match != null)
            this._client.SendPacket((SendPacket) new PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_ACK(0U, match.clan));
          else
            this._client.SendPacket((SendPacket) new PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_ACK(2147483648U));
        }
        else
          this._client.SendPacket((SendPacket) new PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_ACK(2147483648U));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
