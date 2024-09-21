// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.UnBan
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using System;

namespace PointBlank.Game.Data.Chat
{
  public static class UnBan
  {
    public static string UnbanByNick(string str, Account player)
    {
      Account account = AccountManager.getAccount(str.Substring(4), 1, 0);
      return UnBan.BaseUnbanNormal(player, account);
    }

    public static string UnbanById(string str, Account player)
    {
      Account account = AccountManager.getAccount(long.Parse(str.Substring(5)), 0);
      return UnBan.BaseUnbanNormal(player, account);
    }

    public static string SuperUnbanByNick(string str, Account player)
    {
      Account account = AccountManager.getAccount(str.Substring(5), 1, 0);
      return UnBan.BaseUnbanSuper(player, account);
    }

    public static string SuperUnbanById(string str, Account player)
    {
      Account account = AccountManager.getAccount(long.Parse(str.Substring(6)), 0);
      return UnBan.BaseUnbanSuper(player, account);
    }

    private static string BaseUnbanNormal(Account player, Account victim)
    {
      if (victim == null)
        return Translation.GetLabel("PlayerBanUserInvalid");
      if (victim.access == AccessLevel.Banned)
        return Translation.GetLabel("PlayerUnbanAccessInvalid");
      if (victim.ban_obj_id == 0L)
        return Translation.GetLabel("PlayerUnbanNoBan");
      if (victim.player_id == player.player_id)
        return Translation.GetLabel("PlayerUnbanSimilarId");
      return ComDiv.updateDB("ban_history", "expire_date", (object) DateTime.Now, "object_id", (object) victim.ban_obj_id) ? Translation.GetLabel("PlayerUnbanSuccess") : Translation.GetLabel("PlayerUnbanFail");
    }

    private static string BaseUnbanSuper(Account player, Account victim)
    {
      if (victim == null)
        return Translation.GetLabel("PlayerBanUserInvalid");
      if (victim.access != AccessLevel.Banned)
        return Translation.GetLabel("PlayerUnbanAccessInvalid");
      if (victim.player_id == player.player_id)
        return Translation.GetLabel("PlayerUnbanSimilarId");
      if (!ComDiv.updateDB("players", "access_level", (object) 0, "player_id", (object) victim.player_id))
        return Translation.GetLabel("PlayerUnbanFail");
      victim.access = AccessLevel.Normal;
      return Translation.GetLabel("PlayerUnbanSuccess");
    }
  }
}
