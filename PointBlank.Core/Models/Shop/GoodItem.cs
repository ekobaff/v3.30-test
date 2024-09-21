// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Shop.GoodItem
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using PointBlank.Core.Models.Account.Players;

namespace PointBlank.Core.Models.Shop
{
  public class GoodItem
  {
    public int price_gold;
    public int price_cash;
    public int auth_type;
    public int buy_type2;
    public int buy_type3;
    public int id;
    public int tag;
    public int title;
    public int visibility;
    public ItemsModel _item = new ItemsModel()
    {
      _equip = 1
    };
  }
}
