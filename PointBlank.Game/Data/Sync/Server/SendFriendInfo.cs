// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Sync.Server.SendFriendInfo
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Servers;
using PointBlank.Core.Network;

namespace PointBlank.Game.Data.Sync.Server
{
  public class SendFriendInfo
  {
    public static void Load(PointBlank.Game.Data.Model.Account player, Friend friend, int type)
    {
      if (player == null)
        return;
      GameServerModel server = GameSync.GetServer(player._status);
      if (server == null)
        return;
      using (SendGPacket sendGpacket = new SendGPacket())
      {
        sendGpacket.writeH((short) 17);
        sendGpacket.writeQ(player.player_id);
        sendGpacket.writeC((byte) type);
        sendGpacket.writeQ(friend.player_id);
        if (type != 2)
        {
          sendGpacket.writeC((byte) friend.state);
          sendGpacket.writeC(friend.removed);
        }
        GameSync.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
      }
    }
  }
}
