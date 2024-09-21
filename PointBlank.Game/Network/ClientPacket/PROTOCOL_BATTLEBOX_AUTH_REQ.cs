using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Linq;

namespace PointBlank.Game.Network.ClientPacket
{
    internal class PROTOCOL_BATTLEBOX_AUTH_REQ : ReceivePacket
    {
        int obj;

        public PROTOCOL_BATTLEBOX_AUTH_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            obj = readD(); //obj do inventario (deveria ser long), sera que as bbx ficam em outro lugar?
            _ = readD(); //valor do item, usar do json pra evitar erros ou falhas

        }
        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (player._inventory._items.Count >= 500)
                {
                    //0x80001029 EVENT_ERROR_EVENT_BUY_GOODS_INVENTORY_FULL 
                    _client.SendPacket(new PROTOCOL_BATTLEBOX_AUTH_ACK(0x80001029));
                }
                else
                {
#nullable enable
                    ItemsModel? item = player._inventory.getItem((long)obj);
                    BattleBoxInfo? bbx = BattleBoxManager.Get(item?._id ?? 0);
                    if (item != null && bbx != null && (player._tag - bbx.Price) >= 0 && bbx.Itens.Any()
                        && ComDiv.updateDB("players", "tag", player._tag -= bbx.Price, "player_id", player.player_id))
                    {
                        BattleBoxItem? sorted = bbx.GetSortedItem(new Random().Next(1, 100));

                        if (sorted != null)
                        {
                            ItemsModel model = sorted.ToItemModel();
                            ItemsModel current = player._inventory.getItem(sorted.ToItemModel()._id);

                            //Não alterar no inventario se o item atual está perma
                            if (current is null || current._equip != 3)
                                _client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, model));


                        }

                        _client.SendPacket(new PROTOCOL_BATTLEBOX_AUTH_ACK(sorted?.Item ?? 0, sorted?.Count ?? 0, player._tag));
                        _client.SendPacket(new PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK(1, item, _client._player));
                    }
                    else
                        _client.SendPacket(new PROTOCOL_BATTLEBOX_AUTH_ACK(0x80000000));

                }
#nullable restore
            }
            catch (Exception ex)
            {
                Logger.error($"[{GetType().Name}]: {ex.Message}");
            }
        }
    }
}
