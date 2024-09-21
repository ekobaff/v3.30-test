// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.OpenRoomSlot
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Data.Chat
{
  public static class OpenRoomSlot
  {
    public static string OpenSpecificSlot(string str, Account player, PointBlank.Game.Data.Model.Room room)
    {
      int num = int.Parse(str.Substring(6));
      if (num < 1 || num > 16)
        return Translation.GetLabel("OpenRoomSlot_WrongValue");
      int slotIdx = num - 1;
      if (player == null || room == null)
        return Translation.GetLabel("OpenRoomSlot_Fail2");
      Slot slot = room.getSlot(slotIdx);
      if (slot == null || slot.state != SlotState.CLOSE)
        return Translation.GetLabel("OpenRoomSlot_Fail1");
      slot.state = SlotState.EMPTY;
      room.updateSlotsInfo();
      return Translation.GetLabel("OpenRoomSlot_Success1", (object) slotIdx);
    }

    public static string OpenRandomSlot(string str, Account player)
    {
      int num = int.Parse(str.Substring(6));
      int id = num - 1;
      if (num <= 0)
        return Translation.GetLabel("OpenRoomSlot_WrongValue2");
      if (player == null)
        return Translation.GetLabel("OpenRoomSlot_Fail6");
      Channel channel = player.getChannel();
      if (channel == null)
        return Translation.GetLabel("GeneralChannelInvalid");
      PointBlank.Game.Data.Model.Room room = channel.getRoom(id);
      if (room == null)
        return Translation.GetLabel("GeneralRoomNotFounded");
      bool flag = false;
      for (int index = 0; index < 16; ++index)
      {
        Slot slot = room._slots[index];
        if (slot.state == SlotState.CLOSE)
        {
          slot.state = SlotState.EMPTY;
          flag = true;
          break;
        }
      }
      if (flag)
        room.updateSlotsInfo();
      return flag ? Translation.GetLabel("OpenRoomSlot_Success2") : Translation.GetLabel("OpenRoomSlot_Fail3");
    }

    public static string OpenAllSlots(string str, Account player)
    {
      int num = int.Parse(str.Substring(6));
      int id = num - 1;
      if (num <= 0)
        return Translation.GetLabel("OpenRoomSlot_WrongValue2");
      if (player == null)
        return Translation.GetLabel("OpenRoomSlot_Fail6");
      Channel channel = player.getChannel();
      if (channel == null)
        return Translation.GetLabel("OpenRoomSlot_Fail5");
      PointBlank.Game.Data.Model.Room room = channel.getRoom(id);
      if (room == null)
        return Translation.GetLabel("GeneralRoomNotFounded");
      for (int index = 0; index < 16; ++index)
      {
        Slot slot = room._slots[index];
        if (slot.state == SlotState.CLOSE)
          slot.state = SlotState.EMPTY;
      }
      room.updateSlotsInfo();
      return Translation.GetLabel("OpenRoomSlot_Success3");
    }
  }
}
