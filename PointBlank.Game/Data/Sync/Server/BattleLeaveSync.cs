// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Sync.Server.BattleLeaveSync
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Data.Sync.Server
{
  public class BattleLeaveSync
  {
    public static void SendUDPPlayerLeave(Room room, int slotId)
    {
      try
      {
        if (room == null)
          return;
        int playingPlayers = room.getPlayingPlayers(2, SlotState.BATTLE, 0, slotId);
        using (SendGPacket sendGpacket = new SendGPacket())
        {
          sendGpacket.writeH((short) 2);
          sendGpacket.writeD(room.UniqueRoomId);
          sendGpacket.writeD(room.Seed);
          sendGpacket.writeC((byte) slotId);
          sendGpacket.writeC((byte) playingPlayers);
          GameSync.SendPacket(sendGpacket.mstream.ToArray(), room.UdpServer.Connection);
        }
      }
      catch
      {
      }
    }
  }
}
