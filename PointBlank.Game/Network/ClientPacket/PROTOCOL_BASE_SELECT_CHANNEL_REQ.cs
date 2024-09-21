// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BASE_SELECT_CHANNEL_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Xml;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_BASE_SELECT_CHANNEL_REQ : ReceivePacket
  {
    private int channelId;

    public PROTOCOL_BASE_SELECT_CHANNEL_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.channelId = (int) this.readH();

    public override void run()
    {
      Account player = this._client._player;
      if (player == null || player.channelId >= 0)
        return;
      Channel channel = ChannelsXml.getChannel(this.channelId);
      if (channel != null)
      {
        if (this.ChannelRequirementCheck(player, channel))
          this._client.SendPacket((SendPacket) new PROTOCOL_BASE_SELECT_CHANNEL_ACK(2147484162U));
        else if (channel._players.Count >= GameConfig.maxChannelPlayers)
        {
          this._client.SendPacket((SendPacket) new PROTOCOL_BASE_SELECT_CHANNEL_ACK(2147484161U));
        }
        else
        {
          player.channelId = this.channelId;
          this._client.SendPacket((SendPacket) new PROTOCOL_BASE_SELECT_CHANNEL_ACK(player.channelId, 0U));
          player._status.updateChannel((byte) player.channelId);
          player.updateCacheInfo();
        }
      }
      else
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_SELECT_CHANNEL_ACK(2147483648U));
    }

    private bool ChannelRequirementCheck(Account p, Channel ch) => !p.IsGM() && !p.HaveAcessLevel() && (ch._type == 4 && p.clanId == 0 || ch._type == 3 && p._statistic.GetKDRatio() > 40 || ch._type == 2 && p._rank >= 4 || ch._type == 5 && p._rank <= 25 || ch._type == -1);
  }
}
