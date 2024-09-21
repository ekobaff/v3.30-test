// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ClientPacket.PROTOCOL_BASE_GET_SYSTEM_INFO_REQ
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Network.ServerPacket;
using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Network;
using System;
using System.Collections.Generic;

namespace PointBlank.Auth.Network.ClientPacket
{
  public class PROTOCOL_BASE_GET_SYSTEM_INFO_REQ : ReceivePacket
  {
    public PROTOCOL_BASE_GET_SYSTEM_INFO_REQ(AuthClient lc, byte[] buff) => this.makeme(lc, buff);

    public override void read()
    {
      int num = (int) this.readC();
    }

    public override void run()
    {
      try
      {
        PointBlank.Auth.Data.Model.Account player = this._client._player;
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_NOTICE_ACK());
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_URL_LIST_ACK());
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_BOOSTEVENT_INFO_ACK());
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_STEPUP_MODE_INFO_ACK());
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_CHANNELTYPE_CONDITION_ACK());
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_GET_SYSTEM_INFO_ACK());
        if (player == null || !AuthManager.Config.GiftSystem)
          return;
        List<Message> gifts = MessageManager.getGifts(player.player_id);
        if (gifts.Count <= 0)
          return;
        MessageManager.RecicleMessages(player.player_id, gifts);
        if (gifts.Count <= 0)
          return;
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_USER_GIFTLIST_ACK(0, gifts));
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
      }
    }
  }
}
