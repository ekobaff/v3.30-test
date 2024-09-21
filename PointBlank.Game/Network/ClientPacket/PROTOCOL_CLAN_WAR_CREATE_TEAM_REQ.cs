// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CLAN_WAR_CREATE_TEAM_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CLAN_WAR_CREATE_TEAM_REQ : ReceivePacket
  {
    private int formacao;
    private List<int> party = new List<int>();

    public PROTOCOL_CLAN_WAR_CREATE_TEAM_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.formacao = (int) this.readC();

    public override void run()
    {
      Account player = this._client._player;
      if (player == null)
        return;
      Channel channel = player.getChannel();
      if (channel != null && channel._type == 4 && player._room == null)
      {
        if (player._match != null)
          this._client.SendPacket((SendPacket) new PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(2147487879U));
        else if (player.clanId == 0)
        {
          this._client.SendPacket((SendPacket) new PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(2147487835U));
        }
        else
        {
          int num1 = -1;
          int num2 = -1;
          lock (channel._matchs)
          {
            for (int id = 0; id < 250; ++id)
            {
              if (channel.getMatch(id) == null)
              {
                num1 = id;
                break;
              }
            }
            for (int index = 0; index < channel._matchs.Count; ++index)
            {
              Match match = channel._matchs[index];
              if (match.clan._id == player.clanId)
                this.party.Add(match.friendId);
            }
          }
          for (int index = 0; index < 25; ++index)
          {
            if (!this.party.Contains(index))
            {
              num2 = index;
              break;
            }
          }
          if (num1 == -1)
            this._client.SendPacket((SendPacket) new PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(2147487880U));
          else if (num2 == -1)
          {
            this._client.SendPacket((SendPacket) new PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(2147487881U));
          }
          else
          {
            try
            {
              Match match = new Match(ClanManager.getClan(player.clanId))
              {
                _matchId = num1,
                friendId = num2,
                formação = this.formacao,
                channelId = player.channelId,
                serverId = GameConfig.serverId
              };
              match.addPlayer(player);
              channel.AddMatch(match);
              this._client.SendPacket((SendPacket) new PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(0U, match));
              this._client.SendPacket((SendPacket) new PROTOCOL_CLAN_WAR_REGIST_MERCENARY_ACK(match));
            }
            catch (Exception ex)
            {
              Logger.info("PROTOCOL_CLAN_WAR_CREATE_TEAM_REQ: " + ex.ToString());
            }
          }
        }
      }
      else
        this._client.SendPacket((SendPacket) new PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(2147483648U));
    }
  }
}
