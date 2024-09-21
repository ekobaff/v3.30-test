// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.AFKInteraction
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;

namespace PointBlank.Game.Data.Chat
{
  public static class AFKInteraction
  {
    public static string GetAFKCount(string str) => Translation.GetLabel("AFK_Count_Success", (object) GameManager.KickCountActiveClient(double.Parse(str.Substring(9))));

    public static string KickAFKPlayers(string str) => Translation.GetLabel("AFK_Kick_Success", (object) GameManager.KickActiveClient(double.Parse(str.Substring(8))));
  }
}
