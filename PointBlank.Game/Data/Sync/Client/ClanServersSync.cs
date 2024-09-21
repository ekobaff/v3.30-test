// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Sync.Client.ClanServersSync
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;

namespace PointBlank.Game.Data.Sync.Client
{
  public class ClanServersSync
  {
    public static void Load(ReceiveGPacket p)
    {
      int num1 = (int) p.readC();
      int id = p.readD();
      PointBlank.Core.Models.Account.Clan.Clan clan = ClanManager.getClan(id);
      if (num1 == 0)
      {
        if (clan != null)
          return;
        long num2 = p.readQ();
        int num3 = p.readD();
        string str1 = p.readS((int) p.readC());
        string str2 = p.readS((int) p.readC());
        ClanManager.AddClan(new PointBlank.Core.Models.Account.Clan.Clan()
        {
          _id = id,
          _name = str1,
          owner_id = num2,
          _logo = 0U,
          _info = str2,
          creationDate = num3
        });
      }
      else
      {
        if (clan == null)
          return;
        ClanManager.RemoveClan(clan);
      }
    }
  }
}
