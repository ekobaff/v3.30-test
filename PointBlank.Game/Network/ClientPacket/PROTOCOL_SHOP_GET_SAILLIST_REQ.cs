// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_SHOP_GET_SAILLIST_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_SHOP_GET_SAILLIST_REQ : ReceivePacket
    {
        private string md5;

        public PROTOCOL_SHOP_GET_SAILLIST_REQ(GameClient client, byte[] data) => makeme(client, data);

        public override void read() => md5 = readS(32);

        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (player == null)
                    return;
                if (!player.LoadedShop)
                {
                    player.LoadedShop = true;
                    IEnumerable<IEnumerable<ShopData>> shopDatas1 = ShopManager.ShopDataItems.Split(808);
                    IEnumerable<IEnumerable<ShopData>> shopDatas2 = ShopManager.ShopDataGoods.Split(153);
                    IEnumerable<IEnumerable<ShopData>> shopDatas3 = ShopManager.ShopDataItemRepairs.Split(100);
                    IEnumerable<IEnumerable<ShopData>> shopDatas4 = ShopManager.ShopDataMt1.Split(555);
                    IEnumerable<IEnumerable<ShopData>> shopDatas5 = ShopManager.ShopDataMt2.Split(555);
                    foreach (IEnumerable<ShopData> source in shopDatas1)
                    {
                        List<ShopData> list = source.ToList();
                        for (int index = 0; index < list.Count; ++index)
                            _client.SendPacket(new PROTOCOL_AUTH_SHOP_ITEMLIST_ACK(list[index], ShopManager.TotalItems));
                    }
                    foreach (IEnumerable<ShopData> source in shopDatas2)
                    {
                        List<ShopData> list = source.ToList();
                        for (int index = 0; index < list.Count; ++index)
                            _client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODSLIST_ACK(list[index], ShopManager.TotalGoods));
                    }
                    foreach (IEnumerable<ShopData> source in shopDatas3)
                    {
                        List<ShopData> list = source.ToList();
                        for (int index = 0; index < list.Count; ++index)
                            _client.SendPacket(new PROTOCOL_AUTH_SHOP_REPAIRLIST_ACK(list[index], ShopManager.TotalRepairs));
                    }
                    _client.SendPacket(new PROTOCOL_BATTLEBOX_GET_LIST_ACK(BattleBoxManager.GetListACK()));
                    if (player.pc_cafe == 0)
                    {
                        foreach (IEnumerable<ShopData> source in shopDatas4)
                        {
                            List<ShopData> list = source.ToList();
                            for (int index = 0; index < list.Count; ++index)
                                _client.SendPacket(new PROTOCOL_AUTH_SHOP_MATCHINGLIST_ACK(list[index], ShopManager.TotalMatching1));
                        }
                    }
                    else
                    {
                        foreach (IEnumerable<ShopData> source in shopDatas5)
                        {
                            List<ShopData> list = source.ToList();
                            for (int index = 0; index < list.Count; ++index)
                                _client.SendPacket(new PROTOCOL_AUTH_SHOP_MATCHINGLIST_ACK(list[index], ShopManager.TotalMatching2));
                        }
                    }
                }
                if (ShopManager.ReadFile(Environment.CurrentDirectory + "/Data/Shop/Shop.dat") == md5)
                    _client.SendPacket(new PROTOCOL_SHOP_GET_SAILLIST_ACK(false));
                else
                    _client.SendPacket(new PROTOCOL_SHOP_GET_SAILLIST_ACK(true));
            }
            catch (Exception ex)
            {
                Logger.info("PROTOCOL_SHOP_GET_SAILLIST_REQ: " + ex.ToString());
            }
        }
    }
}
