// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.RefillManager
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;

namespace PointBlank.Game.Data.Chat
{
  public static class RefillManager
  {
    public static string RefillPlayer(string str)
    {
      try
      {
        string str1 = str.Substring(7);
        if (str1 == null)
          return Translation.GetLabel("RefillGame");
        if (str1.Length != 14)
          return Translation.GetLabel("RefillGame1");
        return Translation.GetLabel("RefillGame2", (object) str1);
      }
      catch
      {
        return Translation.GetLabel("RefillGame");
      }
    }
  }
}
