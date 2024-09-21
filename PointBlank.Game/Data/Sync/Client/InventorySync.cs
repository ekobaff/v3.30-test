// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Sync.Client.InventorySync
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;

namespace PointBlank.Game.Data.Sync.Client
{
  public class InventorySync
  {
    public static void Load(ReceiveGPacket p)
    {
      long id = p.readQ();
      long num1 = p.readQ();
      int num2 = p.readD();
      int num3 = (int) p.readC();
      int num4 = (int) p.readC();
      long num5 = p.readQ();
      PointBlank.Game.Data.Model.Account account = AccountManager.getAccount(id, true);
      if (account == null)
        return;
      ItemsModel itemsModel = account._inventory.getItem(num1);
      if (itemsModel == null)
        account._inventory.AddItem(new ItemsModel()
        {
          _objId = num1,
          _id = num2,
          _equip = num3,
          _count = num5,
          _category = num4,
          _name = ""
        });
      else
        itemsModel._count = num5;
    }
  }
}
