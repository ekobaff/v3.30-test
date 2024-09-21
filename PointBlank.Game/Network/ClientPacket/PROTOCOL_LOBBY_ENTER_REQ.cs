// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_LOBBY_ENTER_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Threading.Tasks;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_LOBBY_ENTER_REQ : ReceivePacket
    {
        public PROTOCOL_LOBBY_ENTER_REQ(GameClient client, byte[] data) => this.makeme(client, data);

        public override void read()
        {
            int num = (int)this.readC();
        }

        public override void run()
        {
            try
            {
                if (this._client == null)
                    return;
                Account player = this._client._player;
                if (player == null)
                    return;
                player.LastLobbyEnter = DateTime.Now;
                if (player.channelId >= 0)
                    player.getChannel()?.AddPlayer(player.Session);
                Room room = player._room;
                if (room != null)
                {
                    if (player._slotId < 0 || room._state < RoomState.Loading || room._slots[player._slotId].state < SlotState.LOAD)
                        room.RemovePlayer(player, false);
                    else
                        goto label_9;
                }
                AllUtils.syncPlayerToFriends(player, false);
                AllUtils.syncPlayerToClanMembers(player);
                AllUtils.GetXmasReward(player);
            label_9:
                this._client.SendPacket((SendPacket)new PROTOCOL_LOBBY_ENTER_ACK());
                LastFreeBonusCheck(player);
            }
            catch (Exception ex)
            {
                Logger.warning("PROTOCOL_LOBBY_ENTER_REQ: " + ex.ToString());
            }
        }
        private async void LastFreeBonusCheck(Account Player)
        {
            if (Player.LastFreeBonus.Date != DateTime.Now.Date)
            {
                await Task.Delay(1000);

                _client.SendPacket(new PROTOCOL_LOBBY_CHATTING_ACK("Free Bonus", 0U, 5, false, "Jogue uma Partida e receba seu CASH diário GRÁTIS!"));

            }
        }
    }
}
