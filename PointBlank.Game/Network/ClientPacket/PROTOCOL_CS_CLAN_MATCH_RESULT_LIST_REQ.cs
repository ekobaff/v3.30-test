// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CS_CLAN_MATCH_RESULT_LIST_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CS_CLAN_MATCH_RESULT_LIST_REQ : ReceivePacket
  {
    private List<Match> partyList = new List<Match>();
    private int page;

    public PROTOCOL_CS_CLAN_MATCH_RESULT_LIST_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.page = (int) this.readC();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        if (player.clanId > 0)
        {
          Channel channel = player.getChannel();
          if (channel != null && channel._type == 4)
          {
            lock (channel._matchs)
            {
              for (int index = 0; index < channel._matchs.Count; ++index)
              {
                Match match = channel._matchs[index];
                if (match.clan._id == player.clanId)
                  this.partyList.Add(match);
              }
            }
          }
        }
        this._client.SendPacket((SendPacket) new PROTOCOL_CS_CLAN_MATCH_RESULT_LIST_ACK(player.clanId == 0 ? 91 : 0, this.partyList));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
