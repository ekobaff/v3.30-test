// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.LatencyAnalyze
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Data.Chat
{
  public static class LatencyAnalyze
  {
    public static string StartAnalyze(Account player, Room room)
    {
      if (room == null)
        return Translation.GetLabel("GeneralRoomInvalid");
      if (room.getSlot(player._slotId).state != SlotState.BATTLE)
        return Translation.GetLabel("LatencyInfoError");
      player.DebugPing = !player.DebugPing;
      return player.DebugPing ? Translation.GetLabel("LatencyInfoOn") : Translation.GetLabel("LatencyInfoOff");
    }
  }
}
