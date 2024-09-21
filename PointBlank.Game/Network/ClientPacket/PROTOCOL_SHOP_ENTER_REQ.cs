// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_SHOP_ENTER_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Core.Progress;
using PointBlank.Game.Data.Chat;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_SHOP_ENTER_REQ : ReceivePacket
  {
    private string md5;

    public PROTOCOL_SHOP_ENTER_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.md5 = this.readS(32);

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        PointBlank.Game.Data.Model.Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        if (room != null)
        {
          room.changeSlotState(player._slotId, SlotState.SHOP, false);
          room.StopCountDown(player._slotId);
          room.updateSlotsInfo();
        }
        player._topups = PlayerManager.getPlayerTopups(player.player_id);
        if (player._topups.Count > 0)
        {
          for (int index = 0; index < player._topups.Count; ++index)
          {
            PlayerItemTopup topup = player._topups[index];
            if (topup.ItemId != 0)
            {
              this._client.SendPacket((SendPacket) new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, new ItemsModel(topup.ItemId, topup.ItemName, topup.Equip, topup.Count)));
              PlayerManager.DeletePlayerTopup(topup.ObjectId, player.player_id);
            }
          }
        }

                player._makevip = PlayerManager.getPlayerMakeVip(player.player_id);
                if (player._makevip.Count > 0)
                {

                    for (int index = 0; index < player._makevip.Count; ++index)
                    {
                        PlayerMakeVip isvip = player._makevip[index];
                        if (isvip.VipType != 0)
                        {
                            if (PlayerManager.updateAccountVip(player.player_id, isvip.VipType))
                                player.pc_cafe = isvip.VipType;
                            if (PlayerManager.updateAccountCash(player.player_id, player._money + isvip.VipCash))
                                player._money += isvip.VipCash;
                            if (PlayerManager.updateAccountGold(player.player_id, player._gp + isvip.VipGold))
                                player._gp += isvip.VipGold;
                            if (PlayerManager.updateAccountTag(player.player_id, player._tag + isvip.VipTag))
                                player._tag += isvip.VipTag;
                            player.SendPacket((SendPacket)new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, player._gp, player._money, player._tag), false);
                            string msg = "Você recebeu  VIP " + Functions.NameVip(isvip.VipType);
                            msg += "\n - " + isvip.VipCash + " Cash";
                            msg += "\n - " + isvip.VipGold + " Gold";
                            msg += "\n - " + isvip.VipTag + " Tag";

                            MessagBox(player, msg);
                            RefillShop.InstantRefill(player);
                          

                            PlayerManager.DeletePlayerMakeVip(isvip.ObjectId, player.player_id);
                           
                        }
                    }
                }


                this._client.SendPacket((SendPacket) new PROTOCOL_SHOP_ENTER_ACK());
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }

        public static void MessagBox(Account p, string r)
        {
            Message msg = new Message(3)
            {
                sender_name = "[VIP] Combat Global",
                sender_id = 0,
                text = r,
                state = 1
            };

            p.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
            MessageManager.CreateMessage(p.player_id, msg);

        }

    }
}
