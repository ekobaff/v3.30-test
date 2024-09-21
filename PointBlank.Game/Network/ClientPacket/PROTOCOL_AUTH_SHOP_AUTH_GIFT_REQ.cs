﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_AUTH_SHOP_AUTH_GIFT_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Shop;
using PointBlank.Core.Network;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_AUTH_SHOP_AUTH_GIFT_REQ : ReceivePacket
  {
    private int msgId;

    public PROTOCOL_AUTH_SHOP_AUTH_GIFT_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.msgId = this.readD();

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        PointBlank.Game.Data.Model.Account player = this._client._player;
        if (player == null)
          return;
        if (player._inventory._items.Count >= 500)
        {
          this._client.SendPacket((SendPacket) new PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK(2147487785U));
          this._client.SendPacket((SendPacket) new PROTOCOL_AUTH_SHOP_AUTH_GIFT_ACK(2147483648U));
        }
        else
        {
          Message message = MessageManager.getMessage(this.msgId, player.player_id);
          if (message != null && message.type == 2)
          {
            GoodItem good = ShopManager.getGood((int) message.sender_id);
            if (good == null)
              return;
            this._client.SendPacket((SendPacket) new PROTOCOL_AUTH_SHOP_AUTH_GIFT_ACK(1U, good._item, player));
            MessageManager.DeleteMessage(this.msgId, player.player_id);
          }
          else
            this._client.SendPacket((SendPacket) new PROTOCOL_AUTH_SHOP_AUTH_GIFT_ACK(2147483648U));
        }
      }
      catch (Exception ex)
      {
        Logger.info("PROTOCOL_AUTH_SHOP_AUTH_GIFT_REQ: " + ex.ToString());
      }
    }
  }
}
