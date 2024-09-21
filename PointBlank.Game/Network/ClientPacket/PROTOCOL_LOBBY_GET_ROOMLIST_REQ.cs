// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_LOBBY_GET_ROOMLIST_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using PointBlank.Core.Progress;
using PointBlank.Game.Data.Chat;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_LOBBY_GET_ROOMLIST_REQ : ReceivePacket
  {
    public PROTOCOL_LOBBY_GET_ROOMLIST_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        PointBlank.Game.Data.Model.Account player = this._client._player;
        if (player == null)
          return;
        Channel channel = player.getChannel();
        if (channel == null)
          return;
        channel.RemoveEmptyRooms();
        List<Room> rooms = channel._rooms;
        List<PointBlank.Game.Data.Model.Account> waitPlayers = channel.getWaitPlayers();
        int num1 = (int) Math.Ceiling((double) rooms.Count / 15.0);
        int num2 = (int) Math.Ceiling((double) waitPlayers.Count / 10.0);
        if (player.LastRoomPage >= num1)
          player.LastRoomPage = 0;
        if (player.LastPlayerPage >= num2)
          player.LastPlayerPage = 0;
        int count1 = 0;
        int count2 = 0;
        byte[] roomListData = this.GetRoomListData(player.LastRoomPage, ref count1, rooms);
        byte[] playerListData = this.GetPlayerListData(player.LastPlayerPage, ref count2, waitPlayers);
        this._client.SendPacket((SendPacket) new PROTOCOL_LOBBY_GET_ROOMLIST_ACK(rooms.Count, waitPlayers.Count, player.LastRoomPage++, player.LastPlayerPage++, count1, count2, roomListData, playerListData));
        player._topups = PlayerManager.getPlayerTopups(player.player_id);
        if (player._topups.Count > 0)
        {
                    for (int index = 0; index < player._topups.Count; ++index)
                    {
                        PlayerItemTopup topup = player._topups[index];
                        if (topup.ItemId != 0)
                        {
                            this._client.SendPacket((SendPacket)new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, new ItemsModel(topup.ItemId, topup.ItemName, topup.Equip, topup.Count)));
                            PlayerManager.DeletePlayerTopup(topup.ObjectId, player.player_id);
                        }
                    }
        }
        // Caso de erro remover aqui Lobby - Adiciona VIP
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


            }
            catch (Exception ex)
    {
        Logger.warning("PROTOCOL_LOBBY_GET_ROOMLIST_REQ: " + ex.ToString());
      }
    }
        public static void MessagBox(Account p, string r)
        {
            Message msg = new Message(15)
            {
                sender_name = "[VIP] Combat Global",
                sender_id = 0,
                text = r,
                state = 1
            };

            p.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
            MessageManager.CreateMessage(p.player_id, msg);

        }

        private byte[] GetRoomListData(int page, ref int count, List<Room> list)
    {
      using (SendGPacket p = new SendGPacket())
      {
        for (int index = page * 15; index < list.Count; ++index)
        {
          this.WriteRoomData(list[index], p);
          if (++count == 15)
            break;
        }
        return p.mstream.ToArray();
      }
    }

    private void WriteRoomData(Room room, SendGPacket p)
    {
      p.writeD(room._roomId);
      p.writeUnicode(room.name, 46);
      p.writeC((byte) room.mapId);
      p.writeC((byte) room.rule);
      p.writeC(room.stage);
      p.writeC((byte) room.room_type);
      p.writeC((byte) room._state);
      p.writeC((byte) room.getAllPlayers().Count);
      p.writeC((byte) room.getSlotCount());
      p.writeC((byte) room._ping);
      p.writeH((ushort) room.weaponsFlag);
      p.writeH((ushort) room.getFlag());
      p.writeC((byte) room._leader);
      p.writeH((short) 200);
      p.writeC((byte) 0);
    }

    private void WritePlayerData(PointBlank.Game.Data.Model.Account pl, SendGPacket p)
    {
      PointBlank.Core.Models.Account.Clan.Clan clan = ClanManager.getClan(pl.clanId);
      p.writeD(pl.getSessionId());
      p.writeD(clan._logo);
      p.writeC((byte) clan.effect);
      p.writeUnicode(clan._name, 34);
      p.writeH((short) pl.getRank());
      p.writeUnicode(pl.player_name, 66);
      p.writeC((byte) 0);
      p.writeC((byte) 210);
    }

    private byte[] GetPlayerListData(int page, ref int count, List<PointBlank.Game.Data.Model.Account> list)
    {
      using (SendGPacket p = new SendGPacket())
      {
        for (int index = page * 10; index < list.Count; ++index)
        {
          this.WritePlayerData(list[index], p);
          if (++count == 10)
            break;
        }
        return p.mstream.ToArray();
      }
    }
  }
}
