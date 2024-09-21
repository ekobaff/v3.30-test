// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.ChangeServerMode
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Game.Data.Configs;

namespace PointBlank.Game.Data.Chat
{
  public static class ChangeServerMode
  {
    public static string EnableTestMode()
    {
      if (GameConfig.isTestMode)
        return Translation.GetLabel("AlreadyTestModeOn");
      GameConfig.isTestMode = true;
      return Translation.GetLabel("TestModeOn");
    }

    public static string EnablePublicMode()
    {
      if (!GameConfig.isTestMode)
        return Translation.GetLabel("AlreadyTestModeOff");
      GameConfig.isTestMode = false;
      return Translation.GetLabel("TestModeOff");
    }
  }
}
