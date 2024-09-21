// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Sync.Client.FriendSync
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Account;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;

namespace PointBlank.Game.Data.Sync.Client
{
  public class FriendSync
  {
    public static void Load(ReceiveGPacket p)
    {
      long id = p.readQ();
      int num1 = (int) p.readC();
      long num2 = p.readQ();
      Friend friend = (Friend) null;
      if (num1 <= 1)
      {
        int num3 = (int) p.readC();
        bool flag = p.readC() == (byte) 1;
        friend = new Friend(num2)
        {
          state = num3,
          removed = flag
        };
      }
      if (friend == null && num1 <= 1)
        return;
      PointBlank.Game.Data.Model.Account account = AccountManager.getAccount(id, true);
      if (account == null)
        return;
      if (num1 <= 1)
      {
        friend.player.player_name = account.player_name;
        friend.player._rank = account._rank;
        friend.player._isOnline = account._isOnline;
        friend.player._status = account._status;
      }
      if (num1 == 0)
        account.FriendSystem.AddFriend(friend);
      else if (num1 == 1)
      {
        if (account.FriendSystem.GetFriend(num2) == null)
          ;
      }
      else
      {
        if (num1 != 2)
          return;
        account.FriendSystem.RemoveFriend(num2);
      }
    }
  }
}
