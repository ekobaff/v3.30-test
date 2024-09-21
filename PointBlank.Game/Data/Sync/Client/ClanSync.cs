// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Sync.Client.ClanSync
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Data.Sync.Client
{
  public static class ClanSync
  {
    public static void Load(ReceiveGPacket p)
    {
      long id = p.readQ();
      int num1 = (int) p.readC();
      Account account = AccountManager.getAccount(id, true);
      if (account == null || num1 != 3)
        return;
      int num2 = p.readD();
      int num3 = (int) p.readC();
      account.clanId = num2;
      account.clanAccess = num3;
    }
  }
}
