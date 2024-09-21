// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Data.Sync.Client.RemovePlayerSync
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models;
using PointBlank.Battle.Network;

namespace PointBlank.Battle.Data.Sync.Client
{
  public static class RemovePlayerSync
  {
    public static void Load(ReceivePacket p)
    {
      uint UniqueRoomId = p.readUD();
      uint Seed = p.readUD();
      int Slot = (int) p.readC();
      int num = (int) p.readC();
      Room room = RoomsManager.getRoom(UniqueRoomId, Seed);
      if (room == null)
        return;
      if (num == 0)
        RoomsManager.RemoveRoom(UniqueRoomId);
      else
        room.getPlayer(Slot, false)?.ResetAllInfos();
    }
  }
}
