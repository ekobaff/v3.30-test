using System;
using System.Collections.Generic;
using PointBlank.Core;
using PointBlank.Core.Filters;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Xml;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Network.ClientPacket
{
    // Token: 0x020000FE RID: 254
    public class PROTOCOL_BASE_CREATE_NICK_REQ : ReceivePacket
    {
        private string name;
        // Token: 0x06000267 RID: 615 RVA: 0x00003E39 File Offset: 0x00002039
        public PROTOCOL_BASE_CREATE_NICK_REQ(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        // Token: 0x06000268 RID: 616 RVA: 0x0000406B File Offset: 0x0000226B
        public override void read()
        {
            this.name = base.readUnicode((int)(base.readC() * 2));
        }

        // Token: 0x06000269 RID: 617 RVA: 0x00015DD8 File Offset: 0x00013FD8
        public override void run()
        {
            try
            {
                Account player = this._client._player;
                if (player == null || player.player_name.Length > 0 || string.IsNullOrEmpty(this.name) || this.name.Length < GameConfig.minNickSize || this.name.Length > GameConfig.maxNickSize)
                {
                    this._client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(2147487763u, ""));
                }
                else
                {

                    foreach (string value in NickFilter._filter)
                    {
                        if (this.name.Contains(value))
                        {
                            this._client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(2147487763u, ""));
                            return;
                        }
                    }
                    if (!PlayerManager.isPlayerNameExist(this.name))
                    {
                        if (AccountManager.updatePlayerName(this.name, player.player_id))
                        {
                            NickHistoryManager.CreateHistory(player.player_id, player.player_name, this.name, "First nick choosed");
                            player.player_name = this.name;
                            List<ItemsModel> creationAwards = BasicInventoryXml.creationAwards;
                            List<ItemsModel> characters = BasicInventoryXml.Characters;
                            if (creationAwards.Count > 0)
                            {
                                foreach (ItemsModel itemsModel in creationAwards)
                                {
                                    if (itemsModel._id != 0)
                                    {
                                        this._client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, itemsModel));
                                    }
                                }
                            }
                            this._client.SendPacket(new PROTOCOL_SHOP_PLUS_POINT_ACK(player._gp, player._gp, 4));
                            this._client.SendPacket(new PROTOCOL_BASE_QUEST_GET_INFO_ACK(player));
                            this._client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(0u, this.name));
                            if (characters.Count > 0)
                            {
                                foreach (ItemsModel itemsModel2 in characters)
                                {
                                    if (itemsModel2._id != 0)
                                    {
                                        int count = player.Characters.Count;
                                        Character character = new Character();
                                        character.Id = itemsModel2._id;
                                        character.Name = itemsModel2._name;
                                        character.PlayTime = 0;
                                        character.Slot = count++;
                                        character.CreateDate = int.Parse(DateTime.Now.ToString("yyMMddHHmm"));
                                        player.Characters.Add(character);
                                        this._client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, itemsModel2));
                                        if (CharacterManager.Create(character, player.player_id))
                                        {
                                            this._client.SendPacket(new PROTOCOL_CHAR_CREATE_CHARA_ACK(0u, 3, character, player));
                                        }
                                        else
                                        {
                                            this._client.SendPacket(new PROTOCOL_CHAR_CREATE_CHARA_ACK(2147483648u, 0, null, null));
                                        }
                                    }
                                }
                            }
                            this._client.SendPacket(new PROTOCOL_AUTH_FRIEND_INFO_ACK(player.FriendSystem._friends));
                        }
                        else
                        {
                            this._client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(2147487763u, ""));
                        }
                    }
                    else
                    {
                        this._client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(2147483923u, ""));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.warning("PROTOCOL_LOBBY_CREATE_NICK_NAME_REQ: " + ex.ToString());
            }
        }

        // Token: 0x040001BD RID: 445

    }
}
