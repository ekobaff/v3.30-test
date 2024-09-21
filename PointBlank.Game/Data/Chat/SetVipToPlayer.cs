// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.SetVipToPlayer
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using System;

namespace PointBlank.Game.Data.Chat
{
  public static class SetVipToPlayer
  {
    public static string SetVipPlayer(string str)
    {
      string[] strArray = str.Substring(str.IndexOf(" ") + 1).Split(' ');
      long int64 = Convert.ToInt64(strArray[0]);
      int int32 = Convert.ToInt32(strArray[1]);
      Account account = AccountManager.getAccount(int64, 0);
      if (account == null || int32 < 0 || int32 > 2)
        return Translation.GetLabel("[*]SetVip_Fail4");
      if (!PlayerManager.updateAccountVip(account.player_id, int32))
        return Translation.GetLabel("SetVipF");
      try
      {
        account.pc_cafe = int32;
        return Translation.GetLabel("SetVipS", (object) int32, (object) account.player_name);
      }
      catch
      {
        return Translation.GetLabel("SetVipF");
      }
    }
  }
}
