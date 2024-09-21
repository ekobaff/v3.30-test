// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Gift;
using PointBlank.Core.Network;
using PointBlank.Core.Progress;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_REQ : ReceivePacket
  {
    private string Token;
    private uint Error;

    public PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_REQ(GameClient Client, byte[] Buffer) => this.makeme(Client, Buffer);

    public override void read() => this.Token = this.readS((int) this.readC());

    public override void run()
    {
      try
      {
        //this._client.SendPacket((SendPacket) new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("Este sistema ainda não está ativado."));
        PointBlank.Game.Data.Model.Account player = this._client._player;

                if (this.Token == null)
                {
                    this._client.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(2147483648U));
                    this._client.SendPacket((SendPacket)new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("Enter your coupon!"));
                }
                else
                {
                    TicketModel ticketModel = TicketManager.GetTickets().Find((Predicate<TicketModel>)(x => x.Ticket == this.Token));

                    if (ticketModel != null)
                    {

                        this.Error = 0U;
                        if (TicketManager.TicketUsedLog(Token, (long)player.player_id))
                        {
                            this._client.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(2147483648U));
                            this._client.SendPacket((SendPacket)new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("You have already used this coupon!"));
                        }
                        else if (TicketManager.TicketCanUse(Token, ticketModel.MaxUse))
                        {
                            this._client.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(2147483648U));
                            this._client.SendPacket((SendPacket)new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("Activation limit reached!!"));
                        }
                        else
                        {

                            if (ticketModel.Type.HasFlag((Enum)TicketType.ITEM))
                            {
                                ItemsModel itemsModel = new ItemsModel(ticketModel.ItemId, "Ticket Item", ticketModel.Equip, (long)ticketModel.Count);
                                this._client.SendPacket((SendPacket)new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, itemsModel));
                                TicketManager.TicketSaveLog(Token, player.player_id);
                                this._client.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(2147483648U));
                                MessagBox(player, "Ticket successfully activated\nYou received item {col:255, 127, 0, 255}" + ticketModel.ItemName +"{/col} ");
                               
                            }
                            else if (ticketModel.Type.HasFlag((Enum)TicketType.MONEY) || ticketModel.Point == 0 && ticketModel.Cash == 0 && ticketModel.Tag == 0)
                            {

                                if (PlayerManager.updateAccountCash(player.player_id, player._money + ticketModel.Cash))
                                    player._money += ticketModel.Cash;
                                if (PlayerManager.updateAccountGold(player.player_id, player._gp + ticketModel.Point))
                                    player._gp += ticketModel.Point;
                                if (PlayerManager.updateAccountTag(player.player_id, player._tag + ticketModel.Tag))
                                    player._tag += ticketModel.Tag;


                                this._client.SendPacket((SendPacket)new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, player._gp, player._money, player._tag));
                                TicketManager.TicketSaveLog(Token, player.player_id);
                                this._client.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(2147483648U));
                                MessagBox(player, "Ticket successfully activated\nYou received {col:255, 127, 0, 255}\n- "+ticketModel.Cash+" cash\n- "+ticketModel.Point+" gold\n- "+ ticketModel.Tag + " Tag{/col}");
                                
                            }
                            else
                            {
                                this._client.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(2147483648U));
                                this._client.SendPacket((SendPacket)new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("Error activating coupon"));
                            }
                        }



                    }
                    else
                    {
                        //this.Error = 2147483648U;
                        //this._client.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(this.Error));
                        this._client.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(2147483648U));
                        this._client.SendPacket((SendPacket)new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("Error activating coupon"));
                    }
                }

      }
      catch (Exception ex)
      {
        Logger.error("PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_REQ: " + ex.ToString());
      }
    }

        public static void MessagBox(Account p, string r)
        {
            Message msg = new Message(3)
            {
                sender_name = "[Ticket] Combat Global",
                sender_id = 0,
                text = r,
                state = 1
            };

            p.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
            MessageManager.CreateMessage(p.player_id, msg);

        }
    }
}
