// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.SearchSessionClient
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

namespace PointBlank.Game.Data.Chat
{
  public static class SearchSessionClient
  {
    public static string genCode1(string str)
    {
      GameManager.SearchActiveClient(uint.Parse(str.Substring(13)));
      return "";
    }
  }
}
