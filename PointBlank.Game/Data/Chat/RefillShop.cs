// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.RefillShop
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Data.Chat
{
    public static class RefillShop
    {
        public static string SimpleRefill(Account player)
        {
            ShopManager.Reset();
            ShopManager.Load(1);
            BattleBoxManager.Load();
            Logger.warning(Translation.GetLabel("RefillShopWarn", (object)player.player_name));
            return Translation.GetLabel("RefillShopMsg");
        }

        public static string InstantRefill(Account player)
        {
            ShopManager.Reset();
            ShopManager.Load(1);
            BattleBoxManager.Load();
            for (int index = 0; index < ShopManager.ShopDataItems.Count; ++index)
            {
                ShopData shopDataItem = ShopManager.ShopDataItems[index];
                player.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_ITEMLIST_ACK(shopDataItem, ShopManager.TotalItems));
            }
            for (int index = 0; index < ShopManager.ShopDataGoods.Count; ++index)
            {
                ShopData shopDataGood = ShopManager.ShopDataGoods[index];
                player.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_GOODSLIST_ACK(shopDataGood, ShopManager.TotalGoods));
            }
            for (int index = 0; index < ShopManager.ShopDataItemRepairs.Count; ++index)
            {
                ShopData shopDataItemRepair = ShopManager.ShopDataItemRepairs[index];
                player.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_REPAIRLIST_ACK(shopDataItemRepair, ShopManager.TotalRepairs));
            }
            if (player.pc_cafe == 0)
            {
                for (int index = 0; index < ShopManager.ShopDataMt1.Count; ++index)
                {
                    ShopData data = ShopManager.ShopDataMt1[index];
                    player.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_MATCHINGLIST_ACK(data, ShopManager.TotalMatching1));
                }
            }
            else
            {
                for (int index = 0; index < ShopManager.ShopDataMt2.Count; ++index)
                {
                    ShopData data = ShopManager.ShopDataMt2[index];
                    player.SendPacket((SendPacket)new PROTOCOL_AUTH_SHOP_MATCHINGLIST_ACK(data, ShopManager.TotalMatching2));
                }
            }
            player.SendPacket((SendPacket)new PROTOCOL_SHOP_GET_SAILLIST_ACK(true));
            //Logger.warning(Translation.GetLabel("RefillShopWarn", (object) player.player_name));
            return Translation.GetLabel("RefillShopMsg");
        }
    }
}
