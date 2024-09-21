// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.ShopManager
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Shop;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace PointBlank.Core.Managers
{
  public static class ShopManager
  {
    public static List<ItemRepair> ItemRepairs = new List<ItemRepair>();
    public static List<GoodItem> ShopAllList = new List<GoodItem>();
    public static List<GoodItem> ShopBuyableList = new List<GoodItem>();
    public static SortedList<int, GoodItem> ShopUniqueList = new SortedList<int, GoodItem>();
    public static List<ShopData> ShopDataMt1 = new List<ShopData>();
    public static List<ShopData> ShopDataMt2 = new List<ShopData>();
    public static List<ShopData> ShopDataGoods = new List<ShopData>();
    public static List<ShopData> ShopDataItems = new List<ShopData>();
    public static List<ShopData> ShopDataItemRepairs = new List<ShopData>();
    public static int TotalGoods;
    public static int TotalItems;
    public static int TotalMatching1;
    public static int TotalMatching2;
    public static int set4p;
    public static int TotalRepairs;

        // Token: 0x02000080 RID: 128
    // Token: 0x06000364 RID: 868 RVA: 0x0000EA00 File Offset: 0x0000CC00
            public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int limit)
            {
                return from x in list.Select((T item, int inx) => new
                {
                    item,
                    inx
                })
                       group x by x.inx / limit into g
                       select
                           from x in g
                           select x.item;
            }

            // Token: 0x06000365 RID: 869 RVA: 0x0000EA74 File Offset: 0x0000CC74
            public static void Load(int type)
            {
                try
                {
                    using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
                    {
                        npgsqlConnection.Open();
                        NpgsqlCommand npgsqlCommand = npgsqlConnection.CreateCommand();
                        npgsqlCommand.CommandText = "SELECT * FROM shop";
                        npgsqlCommand.CommandType = CommandType.Text;
                        NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader();
                        while (npgsqlDataReader.Read())
                        {
                            GoodItem goodItem = new GoodItem
                            {
                                id = npgsqlDataReader.GetInt32(0),
                                price_gold = npgsqlDataReader.GetInt32(3),
                                price_cash = npgsqlDataReader.GetInt32(4),
                                auth_type = npgsqlDataReader.GetInt32(6),
                                buy_type2 = npgsqlDataReader.GetInt32(7),
                                buy_type3 = npgsqlDataReader.GetInt32(8),
                                tag = npgsqlDataReader.GetInt32(9),
                                title = npgsqlDataReader.GetInt32(10),
                                visibility = npgsqlDataReader.GetInt32(11)
                            };
                            goodItem._item.SetItemId(npgsqlDataReader.GetInt32(1));
                            goodItem._item._name = npgsqlDataReader.GetString(2);
                            goodItem._item._count = (long)npgsqlDataReader.GetInt32(5);
                            int idStatics = ComDiv.getIdStatics(goodItem._item._id, 1);
                            if (type == 1 || (type == 2 && idStatics == 16))
                            {
                                ShopManager.ShopAllList.Add(goodItem);
                                if (goodItem.visibility != 2 && goodItem.visibility != 4)
                                {
                                    ShopManager.ShopBuyableList.Add(goodItem);
                                }
                                if (!ShopManager.ShopUniqueList.ContainsKey(goodItem._item._id) && goodItem.auth_type > 0)
                                {
                                    ShopManager.ShopUniqueList.Add(goodItem._item._id, goodItem);
                                    if (goodItem.visibility == 4)
                                    {
                                        ShopManager.set4p++;
                                    }
                                }
                            }
                        }
                        if (type == 1)
                        {
                            ShopManager.LoadItemRepair();
                            ShopManager.LoadDataMatching1Goods(0);
                            ShopManager.LoadDataMatching2(1);
                            ShopManager.LoadDataItems();
                            ShopManager.LoadDataItemRepairs();
                        }
                        npgsqlCommand.Dispose();
                        npgsqlDataReader.Close();
                        npgsqlConnection.Dispose();
                        npgsqlConnection.Close();
                    }
                }
                catch (Exception ex)
                {
                    Logger.error(ex.ToString());
                }
            }

            // Token: 0x06000366 RID: 870 RVA: 0x0000EC90 File Offset: 0x0000CE90
            public static void LoadItemRepair()
            {
                try
                {
                    using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
                    {
                        npgsqlConnection.Open();
                        NpgsqlCommand npgsqlCommand = npgsqlConnection.CreateCommand();
                        npgsqlCommand.CommandText = "SELECT * FROM shop_item_repair";
                        npgsqlCommand.CommandType = CommandType.Text;
                        NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader();
                        while (npgsqlDataReader.Read())
                        {
                            ItemRepair itemRepair = new ItemRepair
                            {
                                ItemId = npgsqlDataReader.GetInt32(0),
                                Point = npgsqlDataReader.GetInt32(1),
                                Cash = npgsqlDataReader.GetInt32(2),
                                Quantity = npgsqlDataReader.GetInt32(3),
                                Enable = npgsqlDataReader.GetBoolean(4)
                            };
                            if (itemRepair.Enable)
                            {
                                ShopManager.ItemRepairs.Add(itemRepair);
                            }
                        }
                        npgsqlCommand.Dispose();
                        npgsqlDataReader.Close();
                        npgsqlConnection.Dispose();
                        npgsqlConnection.Close();
                    }
                }
                catch (Exception ex)
                {
                    Logger.error(ex.ToString());
                }
            }

            // Token: 0x06000367 RID: 871 RVA: 0x0000ED80 File Offset: 0x0000CF80
            public static void Reset()
            {
                ShopManager.set4p = 0;
                ShopManager.ShopAllList.Clear();
                ShopManager.ShopBuyableList.Clear();
                ShopManager.ShopUniqueList.Clear();
                ShopManager.ShopDataMt1.Clear();
                ShopManager.ShopDataMt2.Clear();
                ShopManager.ShopDataGoods.Clear();
                ShopManager.ShopDataItems.Clear();
                ShopManager.ShopDataItemRepairs.Clear();
                ShopManager.ItemRepairs.Clear();
                ShopManager.TotalGoods = 0;
                ShopManager.TotalItems = 0;
                ShopManager.TotalMatching1 = 0;
                ShopManager.TotalMatching2 = 0;
            }

            // Token: 0x06000368 RID: 872 RVA: 0x0000EE08 File Offset: 0x0000D008
            private static void LoadDataMatching1Goods(int cafe)
            {
                List<GoodItem> list = new List<GoodItem>();
                List<GoodItem> list2 = new List<GoodItem>();
                List<GoodItem> shopAllList = ShopManager.ShopAllList;
                lock (shopAllList)
                {
                    for (int i = 0; i < ShopManager.ShopAllList.Count; i++)
                    {
                        GoodItem goodItem = ShopManager.ShopAllList[i];
                        if (goodItem._item._count != 0L)
                        {
                            if ((goodItem.tag != 4 || cafe != 0) && ((goodItem.tag == 4 && cafe > 0) || goodItem.visibility != 2))
                            {
                                list.Add(goodItem);
                            }
                            if (goodItem.visibility < 2 || goodItem.visibility == 4)
                            {
                                list2.Add(goodItem);
                            }
                        }
                    }
                }
                ShopManager.TotalMatching1 = list.Count;
                ShopManager.TotalGoods = list2.Count;
                int num = (int)Math.Ceiling((double)list.Count / 555.0);
                int itemsCount = 0;
                for (int j = 0; j < num; j++)
                {
                    byte[] matchingData = ShopManager.getMatchingData(555, j, ref itemsCount, list);
                    ShopData item = new ShopData
                    {
                        Buffer = matchingData,
                        ItemsCount = itemsCount,
                        Offset = j * 555
                    };
                    ShopManager.ShopDataMt1.Add(item);
                }
                num = (int)Math.Ceiling((double)list2.Count / 153.0);
                for (int k = 0; k < num; k++)
                {
                    byte[] goodsData = ShopManager.getGoodsData(153, k, ref itemsCount, list2);
                    ShopData item2 = new ShopData
                    {
                        Buffer = goodsData,
                        ItemsCount = itemsCount,
                        Offset = k * 153
                    };
                    ShopManager.ShopDataGoods.Add(item2);
                }
            }

            // Token: 0x06000369 RID: 873 RVA: 0x0000EFBC File Offset: 0x0000D1BC
            private static void LoadDataMatching2(int cafe)
            {
                List<GoodItem> list = new List<GoodItem>();
                List<GoodItem> shopAllList = ShopManager.ShopAllList;
                lock (shopAllList)
                {
                    for (int i = 0; i < ShopManager.ShopAllList.Count; i++)
                    {
                        GoodItem goodItem = ShopManager.ShopAllList[i];
                        if (goodItem._item._count != 0L && (goodItem.tag != 4 || cafe != 0) && ((goodItem.tag == 4 && cafe > 0) || goodItem.visibility != 2))
                        {
                            list.Add(goodItem);
                        }
                    }
                }
                ShopManager.TotalMatching2 = list.Count;
                int num = (int)Math.Ceiling((double)list.Count / 555.0);
                int itemsCount = 0;
                for (int j = 0; j < num; j++)
                {
                    byte[] matchingData = ShopManager.getMatchingData(555, j, ref itemsCount, list);
                    ShopData item = new ShopData
                    {
                        Buffer = matchingData,
                        ItemsCount = itemsCount,
                        Offset = j * 555
                    };
                    ShopManager.ShopDataMt2.Add(item);
                }
            }

            // Token: 0x0600036A RID: 874 RVA: 0x0000F0D8 File Offset: 0x0000D2D8
            private static void LoadDataItems()
            {
                List<GoodItem> list = new List<GoodItem>();
                SortedList<int, GoodItem> shopUniqueList = ShopManager.ShopUniqueList;
                lock (shopUniqueList)
                {
                    for (int i = 0; i < ShopManager.ShopUniqueList.Values.Count; i++)
                    {
                        GoodItem goodItem = ShopManager.ShopUniqueList.Values[i];
                        if (goodItem.visibility != 1 && goodItem.visibility != 3)
                        {
                            list.Add(goodItem);
                        }
                    }
                }
                ShopManager.TotalItems = list.Count;
                int num = (int)Math.Ceiling((double)list.Count / 808.0);
                int itemsCount = 0;
                for (int j = 0; j < num; j++)
                {
                    byte[] itemsData = ShopManager.getItemsData(808, j, ref itemsCount, list);
                    ShopData item = new ShopData
                    {
                        Buffer = itemsData,
                        ItemsCount = itemsCount,
                        Offset = j * 808
                    };
                    ShopManager.ShopDataItems.Add(item);
                }
            }

            // Token: 0x0600036B RID: 875 RVA: 0x0000F1DC File Offset: 0x0000D3DC
            private static void LoadDataItemRepairs()
            {
                List<ItemRepair> list = new List<ItemRepair>();
                List<ItemRepair> itemRepairs = ShopManager.ItemRepairs;
                lock (itemRepairs)
                {
                    for (int i = 0; i < ShopManager.ItemRepairs.Count; i++)
                    {
                        ItemRepair item = ShopManager.ItemRepairs[i];
                        list.Add(item);
                    }
                }
                ShopManager.TotalRepairs = list.Count;
                int num = (int)Math.Ceiling((double)list.Count / 100.0);
                int itemsCount = 0;
                for (int j = 0; j < num; j++)
                {
                    byte[] repairsData = ShopManager.getRepairsData(100, j, ref itemsCount, list);
                    ShopData item2 = new ShopData
                    {
                        Buffer = repairsData,
                        ItemsCount = itemsCount,
                        Offset = j * 100
                    };
                    ShopManager.ShopDataItemRepairs.Add(item2);
                }
            }

            // Token: 0x0600036C RID: 876 RVA: 0x0000F2BC File Offset: 0x0000D4BC
            private static byte[] getItemsData(int maximum, int page, ref int count, List<GoodItem> list)
            {
                count = 0;
                byte[] result;
                using (SendGPacket sendGPacket = new SendGPacket())
                {
                    for (int i = page * maximum; i < list.Count; i++)
                    {
                        ShopManager.WriteItemsData(list[i], sendGPacket);
                        int num = count + 1;
                        count = num;
                        if (num == maximum)
                        {
                            break;
                        }
                    }
                    result = sendGPacket.mstream.ToArray();
                }
                return result;
            }

            // Token: 0x0600036D RID: 877 RVA: 0x0000F328 File Offset: 0x0000D528
            private static byte[] getRepairsData(int maximum, int page, ref int count, List<ItemRepair> list)
            {
                count = 0;
                byte[] result;
                using (SendGPacket sendGPacket = new SendGPacket())
                {
                    for (int i = page * maximum; i < list.Count; i++)
                    {
                        ShopManager.WriteRepairsData(list[i], sendGPacket);
                        int num = count + 1;
                        count = num;
                        if (num == maximum)
                        {
                            break;
                        }
                    }
                    result = sendGPacket.mstream.ToArray();
                }
                return result;
            }

            // Token: 0x0600036E RID: 878 RVA: 0x0000F394 File Offset: 0x0000D594
            private static byte[] getGoodsData(int maximum, int page, ref int count, List<GoodItem> list)
            {
                count = 0;
                byte[] result;
                using (SendGPacket sendGPacket = new SendGPacket())
                {
                    for (int i = page * maximum; i < list.Count; i++)
                    {
                        ShopManager.WriteGoodsData(list[i], sendGPacket);
                        int num = count + 1;
                        count = num;
                        if (num == maximum)
                        {
                            break;
                        }
                    }
                    result = sendGPacket.mstream.ToArray();
                }
                return result;
            }

            // Token: 0x0600036F RID: 879 RVA: 0x0000F400 File Offset: 0x0000D600
            private static byte[] getMatchingData(int maximum, int page, ref int count, List<GoodItem> list)
            {
                count = 0;
                byte[] result;
                using (SendGPacket sendGPacket = new SendGPacket())
                {
                    for (int i = page * maximum; i < list.Count; i++)
                    {
                        ShopManager.WriteMatchData(list[i], sendGPacket);
                        int num = count + 1;
                        count = num;
                        if (num == maximum)
                        {
                            break;
                        }
                    }
                    result = sendGPacket.mstream.ToArray();
                }
                return result;
            }

            // Token: 0x06000370 RID: 880 RVA: 0x0000F46C File Offset: 0x0000D66C
            private static void WriteItemsData(GoodItem good, SendGPacket p)
            {
                p.writeD(good._item._id);
                p.writeC((byte)good.auth_type);
                p.writeC((byte)good.buy_type2);
                p.writeC((byte)good.buy_type3);
                p.writeC((byte)good.title);
                //	p.writeC((good.title != 0) ? 2 : 0));
                p.writeC((byte)(good.title != 0 ? 2 : 0));
                p.writeH(0);
            }

            // Token: 0x06000371 RID: 881 RVA: 0x00004165 File Offset: 0x00002365
            private static void WriteRepairsData(ItemRepair repair, SendGPacket p)
            {
                p.writeD(repair.ItemId);
                p.writeD(repair.Point);
                p.writeD(repair.Cash);
                p.writeD(repair.Quantity);
            }

            // Token: 0x06000372 RID: 882 RVA: 0x0000F4D8 File Offset: 0x0000D6D8
            private static void WriteGoodsData(GoodItem good, SendGPacket p)
            {
                p.writeD(good.id);
                p.writeC(1);
                p.writeC((byte)(good.visibility == 4 ? 4 : 1));
                p.writeD(good.price_gold);
                p.writeD(good.price_cash);
                p.writeD(0);
                p.writeC((byte)good.tag);
                p.writeB(new byte[39]);
            }

            // Token: 0x06000373 RID: 883 RVA: 0x00004197 File Offset: 0x00002397
            private static void WriteMatchData(GoodItem good, SendGPacket p)
            {
                p.writeD(good.id);
                p.writeD(good._item._id);
                p.writeD((int)good._item._count);
                p.writeD(0);
            }

            // Token: 0x06000374 RID: 884 RVA: 0x0000F548 File Offset: 0x0000D748
            public static bool IsBlocked(string txt, List<int> items)
            {
                SortedList<int, GoodItem> shopUniqueList = ShopManager.ShopUniqueList;
                lock (shopUniqueList)
                {
                    for (int i = 0; i < ShopManager.ShopUniqueList.Values.Count; i++)
                    {
                        GoodItem goodItem = ShopManager.ShopUniqueList.Values[i];
                        if (!items.Contains(goodItem._item._id) && goodItem._item._name.Contains(txt))
                        {
                            items.Add(goodItem._item._id);
                        }
                    }
                }
                return false;
            }

            // Token: 0x06000375 RID: 885 RVA: 0x0000F5E4 File Offset: 0x0000D7E4
            public static GoodItem getGood(int goodId)
            {
                if (goodId == 0)
                {
                    return null;
                }
                List<GoodItem> shopAllList = ShopManager.ShopAllList;
                lock (shopAllList)
                {
                    for (int i = 0; i < ShopManager.ShopAllList.Count; i++)
                    {
                        GoodItem goodItem = ShopManager.ShopAllList[i];
                        if (goodItem.id == goodId)
                        {
                            return goodItem;
                        }
                    }
                }
                return null;
            }

            // Token: 0x06000376 RID: 886 RVA: 0x0000F658 File Offset: 0x0000D858
            public static GoodItem getItemId(int ItemId)
            {
                if (ItemId == 0)
                {
                    return null;
                }
                List<GoodItem> shopAllList = ShopManager.ShopAllList;
                lock (shopAllList)
                {
                    for (int i = 0; i < ShopManager.ShopAllList.Count; i++)
                    {
                        GoodItem goodItem = ShopManager.ShopAllList[i];
                        if (goodItem._item._id == ItemId)
                        {
                            return goodItem;
                        }
                    }
                }
                return null;
            }

            // Token: 0x06000377 RID: 887 RVA: 0x0000F6D0 File Offset: 0x0000D8D0
            public static List<GoodItem> getGoods(List<CartGoods> ShopCart, out int GoldPrice, out int CashPrice)
            {
                GoldPrice = 0;
                CashPrice = 0;
                List<GoodItem> list = new List<GoodItem>();
                if (ShopCart.Count == 0)
                {
                    return list;
                }
                List<GoodItem> shopBuyableList = ShopManager.ShopBuyableList;
                lock (shopBuyableList)
                {
                    for (int i = 0; i < ShopManager.ShopBuyableList.Count; i++)
                    {
                        GoodItem goodItem = ShopManager.ShopBuyableList[i];
                        for (int j = 0; j < ShopCart.Count; j++)
                        {
                            CartGoods cartGoods = ShopCart[j];
                            if (cartGoods.GoodId == goodItem.id)
                            {
                                list.Add(goodItem);
                                if (cartGoods.BuyType == 1)
                                {
                                    GoldPrice += goodItem.price_gold;
                                }
                                else if (cartGoods.BuyType == 2)
                                {
                                    CashPrice += goodItem.price_cash;
                                }
                            }
                        }
                    }
                }
                return list;
            }

            // Token: 0x06000378 RID: 888 RVA: 0x0000F7AC File Offset: 0x0000D9AC
            public static string ReadFile(string Path)
            {
                string result = "";
                using (MD5 md = MD5.Create())
                {
                    using (FileStream fileStream = File.OpenRead(Path))
                    {
                        result = BitConverter.ToString(md.ComputeHash(fileStream)).Replace("-", string.Empty);
                        fileStream.Close();
                    }
                }
                return result;
            }


   //public static bool IsBlocked(string txt, List<int> items)
   // {
   //   lock (ShopManager.ShopUniqueList)
   //   {
   //     for (int index = 0; index < ShopManager.ShopUniqueList.Values.Count; ++index)
   //     {
   //       GoodItem goodItem = ShopManager.ShopUniqueList.Values[index];
   //       if (!items.Contains(goodItem._item._id) && goodItem._item._name.Contains(txt))
   //         items.Add(goodItem._item._id);
   //     }
   //   }
   //   return false;
   // }

  }
}
