// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_INVENTORY_GET_INFO_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Shop;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Sync.Server;
using System;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_INVENTORY_GET_INFO_ACK : SendPacket
  {
    private int Type;
    private List<ItemsModel> Items = new List<ItemsModel>();

    public PROTOCOL_INVENTORY_GET_INFO_ACK(int Type, PointBlank.Game.Data.Model.Account Player, ItemsModel Item)
    {
      this.Type = Type;
      this.AddItems(Player, Item);
    }

    public PROTOCOL_INVENTORY_GET_INFO_ACK(int Type, PointBlank.Game.Data.Model.Account Player, List<GoodItem> Items)
    {
      this.Type = Type;
      this.AddItems(Player, Items);
    }

    public override void write()
    {
      this.writeH((short) 3334);
      this.writeH((short) 0);
      this.writeH((short) this.Items.Count);
      for (int index = 0; index < this.Items.Count; ++index)
      {
        ItemsModel itemsModel = this.Items[index];
        this.writeD((uint) itemsModel._objId);
        this.writeD(itemsModel._id);
        this.writeC((byte) itemsModel._equip);
        this.writeD((int) itemsModel._count);
      }
      this.writeC((byte) this.Type);
    }

    private void AddItems(PointBlank.Game.Data.Model.Account Player, ItemsModel Item)
    {
      try
      {
        ItemsModel modelo = new ItemsModel(Item)
        {
          _objId = Item._objId
        };
        if (this.Type == 0)
          PlayerManager.tryCreateItem(modelo, Player._inventory, Player.player_id);
        SendItemInfo.LoadItem(Player, modelo);
        this.Items.Add(modelo);
      }
      catch (Exception ex)
      {
        Player.Close(0);
        Logger.warning("PROTOCOL_INVENTORY_GET_INFO_ACK: " + ex.ToString());
      }
    }

    private void AddItems(PointBlank.Game.Data.Model.Account Player, List<GoodItem> Goods)
    {
      GoodItem goodItem = (GoodItem) null;
      try
      {
        foreach (GoodItem good in Goods)
        {
          goodItem = good;
          Player._inventory.getItem(good._item._id);
          ItemsModel modelo = new ItemsModel(good._item);
          if (this.Type == 0)
            PlayerManager.tryCreateItem(modelo, Player._inventory, Player.player_id);
          SendItemInfo.LoadItem(Player, modelo);
          this.Items.Add(modelo);
        }
      }
      catch (Exception ex)
      {
        Logger.warning("PROTOCOL_INVENTORY_GET_INFO_ACK: " + ex.ToString());
      }
    }
  }
}
