// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.SetCashToPlayer
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Sync.Server;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Data.Chat
{
  public static class SetCashToPlayer
  {
    public static string SetCashPlayer(string str)
    {
      string[] strArray = str.Substring(str.IndexOf(" ") + 1).Split(' ');
      long int64 = Convert.ToInt64(strArray[0]);
      int int32 = Convert.ToInt32(strArray[1]);
      Account account = AccountManager.getAccount(int64, 0);
      if (account == null || account._money + int32 > 999999999 || int32 < 0)
        return Translation.GetLabel("[*]SendCash_Fail4");
      if (!PlayerManager.updateAccountCash(account.player_id, account._money = int32))
        return Translation.GetLabel("GiveCashFail2");
      account._money = int32;
      account.SendPacket((SendPacket) new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, account._gp, account._money, account._tag), false);
      SendItemInfo.LoadGoldCash(account);
      return Translation.GetLabel("GiveCashSuccessD", (object) account._money, (object) account.player_name);
    }
  }
}
