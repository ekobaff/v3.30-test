// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Data.Sync.Server.SendRefresh
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Data.Managers;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Servers;
using PointBlank.Core.Network;
using PointBlank.Core.Xml;

namespace PointBlank.Auth.Data.Sync.Server
{
  public static class SendRefresh
  {
    public static void RefreshAccount(PointBlank.Auth.Data.Model.Account player, bool isConnect)
    {
      AuthSync.UpdateGSCount(0);
      AccountManager.getInstance().getFriendlyAccounts(player.FriendSystem);
      for (int index = 0; index < player.FriendSystem._friends.Count; ++index)
      {
        Friend friend = player.FriendSystem._friends[index];
        PlayerInfo player1 = friend.player;
        if (player1 != null)
        {
          GameServerModel server = ServersXml.getServer((int) player1._status.serverId);
          if (server != null)
            SendRefresh.SendRefreshPacket(0, player.player_id, friend.player_id, isConnect, server);
        }
      }
      if (player.clan_id <= 0)
        return;
      for (int index = 0; index < player._clanPlayers.Count; ++index)
      {
        PointBlank.Auth.Data.Model.Account clanPlayer = player._clanPlayers[index];
        if (clanPlayer != null && clanPlayer._isOnline)
        {
          GameServerModel server = ServersXml.getServer((int) clanPlayer._status.serverId);
          if (server != null)
            SendRefresh.SendRefreshPacket(1, player.player_id, clanPlayer.player_id, isConnect, server);
        }
      }
    }

    public static void SendRefreshPacket(
      int type,
      long playerId,
      long memberId,
      bool isConnect,
      GameServerModel gs)
    {
      using (SendGPacket sendGpacket = new SendGPacket())
      {
        sendGpacket.writeH((short) 11);
        sendGpacket.writeC((byte) type);
        sendGpacket.writeC(isConnect);
        sendGpacket.writeQ(playerId);
        sendGpacket.writeQ(memberId);
        AuthSync.SendPacket(sendGpacket.mstream.ToArray(), gs.Connection);
      }
    }
  }
}
