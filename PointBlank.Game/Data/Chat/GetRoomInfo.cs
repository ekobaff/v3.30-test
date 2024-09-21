// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.GetRoomInfo
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Data.Chat
{
  public static class GetRoomInfo
  {
    public static string GetSlotStats(string str, Account player, PointBlank.Game.Data.Model.Room room)
    {
      int slotIdx = int.Parse(str.Substring(5)) - 1;
      string str1 = "Infomation:";
      if (room == null)
        return "Sala invalid. [Server]";
      Slot slot = room.getSlot(slotIdx);
      if (slot == null)
        return "Slot invalid. [Server]";
      string msg = str1 + "\nIndex: " + slot._id.ToString() + "\nTeam: " + slot._team.ToString() + "\nFlag: " + slot.Flag.ToString() + "\nAccountId: " + slot._playerId.ToString() + "\nState: " + slot.state.ToString() + "\nMissions: " + (slot.Missions != null ? "Valid" : "Null");
      player.SendPacket((SendPacket) new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(msg));
      return "Successfully generated slot logs. [Server]";
    }
  }
}
