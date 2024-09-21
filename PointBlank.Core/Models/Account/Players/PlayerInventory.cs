// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Account.Players.PlayerInventory
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using PointBlank.Core.Xml;
using System.Collections.Generic;

namespace PointBlank.Core.Models.Account.Players
{
  public class PlayerInventory
  {
    public List<ItemsModel> _items = new List<ItemsModel>();

    public ItemsModel getItem(int id)
    {
      lock (this._items)
      {
        for (int index = 0; index < this._items.Count; ++index)
        {
          ItemsModel itemsModel = this._items[index];
          if (itemsModel._id == id)
            return itemsModel;
        }
      }
      return (ItemsModel) null;
    }

    public ItemsModel getItem(long obj)
    {
      lock (this._items)
      {
        for (int index = 0; index < this._items.Count; ++index)
        {
          ItemsModel itemsModel = this._items[index];
          if (itemsModel._objId == obj)
            return itemsModel;
        }
      }
      return (ItemsModel) null;
    }

    public void LoadBasicItems()
    {
      lock (this._items)
        this._items.AddRange((IEnumerable<ItemsModel>) BasicInventoryXml.basic);
    }

        public void LoadVipBasic()
        {
            List<ItemsModel> items = this._items;
            lock (items)
            {
                this._items.AddRange((IEnumerable < ItemsModel > ) CafeInventoryXml.vipbasic);
            }
        }
        public void LoadVipPlus()
        {
            List<ItemsModel> items = this._items;
            lock (items)
            {
                this._items.AddRange((IEnumerable<ItemsModel>)CafeInventoryXml.vipplus);
            }
        }
        public void LoadVipMaster()
        {
            List<ItemsModel> items = this._items;
            lock (items)
            {
                this._items.AddRange((IEnumerable<ItemsModel>)CafeInventoryXml.vipmaster);
            }
        }
        public void LoadVipCombat()
        {
            List<ItemsModel> items = this._items;
            lock (items)
            {
                this._items.AddRange((IEnumerable<ItemsModel>)CafeInventoryXml.vipcombat);
            }
        }
        public void LoadVipExtreme()
        {
            List<ItemsModel> items = this._items;
            lock (items)
            {
                this._items.AddRange((IEnumerable<ItemsModel>)CafeInventoryXml.vipextreme);
            }
        }
        public void LoadVipBooster()
        {
            List<ItemsModel> items = this._items;
            lock (items)
            {
                this._items.AddRange((IEnumerable<ItemsModel>)CafeInventoryXml.vipbooster);
            }
        }

        public List<ItemsModel> getItemsByType(int type)
    {
      List<ItemsModel> itemsByType = new List<ItemsModel>();
      lock (this._items)
      {
        for (int index = 0; index < this._items.Count; ++index)
        {
          ItemsModel itemsModel = this._items[index];
          if (itemsModel._category == type || itemsModel._id > 1600000 && itemsModel._id < 1700000 && type == 4)
            itemsByType.Add(itemsModel);
        }
      }
      return itemsByType;
    }

    public void RemoveItem(long obj)
    {
      lock (this._items)
      {
        for (int index = 0; index < this._items.Count; ++index)
        {
          if (this._items[index]._objId == obj)
          {
            this._items.RemoveAt(index);
            break;
          }
        }
      }
    }

    public bool RemoveItem(ItemsModel item)
    {
      lock (this._items)
        return this._items.Remove(item);
    }

    public void AddItem(ItemsModel item)
    {
      lock (this._items)
        this._items.Add(item);
    }
  }
}
