// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Sync.Server.SendItemInfo
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Servers;
using PointBlank.Core.Network;

namespace PointBlank.Game.Data.Sync.Server
{
  public class SendItemInfo
  {
    public static void LoadItem(PointBlank.Game.Data.Model.Account player, ItemsModel item)
    {
      if (player == null || player._status.serverId == (byte) 0)
        return;
      GameServerModel server = GameSync.GetServer(player._status);
      if (server == null)
        return;
      using (SendGPacket sendGpacket = new SendGPacket())
      {
        sendGpacket.writeH((short) 18);
        sendGpacket.writeQ(player.player_id);
        sendGpacket.writeQ(item._objId);
        sendGpacket.writeD(item._id);
        sendGpacket.writeC((byte) item._equip);
        sendGpacket.writeC((byte) item._category);
        sendGpacket.writeQ(item._count);
        GameSync.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
      }
    }

    public static void LoadGoldCash(PointBlank.Game.Data.Model.Account player)
    {
      if (player == null)
        return;
      GameServerModel server = GameSync.GetServer(player._status);
      if (server == null)
        return;
      using (SendGPacket sendGpacket = new SendGPacket())
      {
        sendGpacket.writeH((short) 19);
        sendGpacket.writeQ(player.player_id);
        sendGpacket.writeC((byte) 0);
        sendGpacket.writeC((byte) player._rank);
        sendGpacket.writeD(player._gp);
        sendGpacket.writeD(player._money);
        GameSync.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
      }
    }
  }
}
