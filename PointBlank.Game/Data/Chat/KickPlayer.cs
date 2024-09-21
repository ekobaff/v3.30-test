// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.KickPlayer
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Data.Chat
{
  public static class KickPlayer
  {
    public static string KickByNick(string str, Account player)
    {
      Account account = AccountManager.getAccount(str.Substring(3), 1, 0);
      return KickPlayer.BaseKick(player, account);
    }

    public static string KickById(string str, Account player)
    {
      Account account = AccountManager.getAccount(long.Parse(str.Substring(4)), 0);
      return KickPlayer.BaseKick(player, account);
    }

    private static string BaseKick(Account player, Account victim)
    {
      if (victim == null)
        return Translation.GetLabel("PlayerKickNotFound");
      if (victim.access > player.access)
        return Translation.GetLabel("PlayerBanAccessInvalid");
      if (victim.player_id == player.player_id)
        return Translation.GetLabel("PlayerKickKickYourself");
      if (victim._connection != null)
      {
        victim.SendPacket((SendPacket) new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(2), false);
        victim.Close(1000, true);
        return Translation.GetLabel("PlayerKickSuccess", (object) victim.player_name);
      }
      return Translation.GetLabel("PlayerKickOffline", (object) victim.player_name);
    }
  }
}
