﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CS_COMMISSION_REGULAR_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Sync.Server;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CS_COMMISSION_REGULAR_REQ : ReceivePacket
  {
    private uint result;

    public PROTOCOL_CS_COMMISSION_REGULAR_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      PointBlank.Game.Data.Model.Account player = this._client._player;
      if (player == null)
        return;
      PointBlank.Core.Models.Account.Clan.Clan clan = ClanManager.getClan(player.clanId);
      if (clan._id == 0 || (player.clanAccess < 1 || player.clanAccess > 2) && clan.owner_id != this._client.player_id)
      {
        this.result = 2147487833U;
      }
      else
      {
        int num = (int) this.readC();
        for (int index = 0; index < num; ++index)
        {
          PointBlank.Game.Data.Model.Account account = AccountManager.getAccount(this.readQ(), 0);
          if (account != null && account.clanId == clan._id && account.clanAccess == 2 && ComDiv.updateDB("players", "clanaccess", (object) 3, "player_id", (object) account.player_id))
          {
            account.clanAccess = 3;
            SendClanInfo.Load(account, (PointBlank.Game.Data.Model.Account) null, 3);
            if (MessageManager.getMsgsCount(account.player_id) < 100)
            {
              Message message = this.CreateMessage(clan, account.player_id, this._client.player_id);
              if (message != null && account._isOnline)
                account.SendPacket((SendPacket) new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(message), false);
            }
            if (account._isOnline)
              account.SendPacket((SendPacket) new PROTOCOL_CS_COMMISSION_REGULAR_RESULT_ACK(), false);
            ++this.result;
          }
        }
      }
    }

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        this._client.SendPacket((SendPacket) new PROTOCOL_CS_COMMISSION_REGULAR_ACK(this.result));
      }
      catch (Exception ex)
      {
        Logger.info("PROTOCOL_CS_COMMISSION_REGULAR_REQ: " + ex.ToString());
      }
    }

    private Message CreateMessage(PointBlank.Core.Models.Account.Clan.Clan clan, long owner, long senderId)
    {
      Message msg = new Message(15.0)
      {
        sender_name = clan._name,
        sender_id = senderId,
        clanId = clan._id,
        type = 4,
        state = 1,
        cB = NoteMessageClan.Regular
      };
      return !MessageManager.CreateMessage(owner, msg) ? (Message) null : msg;
    }
  }
}
