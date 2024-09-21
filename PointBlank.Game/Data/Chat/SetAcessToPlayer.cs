// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.RefillManager2
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Enums;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using System;

namespace PointBlank.Game.Data.Chat
{
  public static class RefillManager2
  {
    public static string SetAcessPlayer(string str)
    {
      string[] strArray = str.Substring(str.IndexOf(" ") + 1).Split(' ');
      long int64 = Convert.ToInt64(strArray[0]);
      int int32 = Convert.ToInt32(strArray[1]);
      Account account = AccountManager.getAccount(int64, 0);
      if (account == null || int32 < 0 || int32 > 6)
        return Translation.GetLabel("[*]SetAcess_Fail4");
      if (!PlayerManager.updateAccountAccess(account.player_id, int32))
        return Translation.GetLabel("SetAcessF");
      try
      {
        account.access = (AccessLevel) int32;
        return Translation.GetLabel("SetAcessS", (object) int32, (object) account.player_name);
      }
      catch
      {
        return Translation.GetLabel("SetAcessF");
      }
    }
  }
}
