// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.ChangeChannelNotice
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Xml;

namespace PointBlank.Game.Data.Chat
{
  public static class ChangeChannelNotice
  {
    public static string SetChannelNotice(string str)
    {
      int length = str.IndexOf(" ");
      if (length == -1)
        return Translation.GetLabel("ChangeChAnnounceFail");
      int num = int.Parse(str.Substring(7, length));
      if (num < 1)
        return Translation.GetLabel("ChangeChAnnounceFail2");
      int channelId = num - 1;
      string text = str.Substring(length + 1);
      if (!ChannelsXml.updateNotice(GameConfig.serverId, channelId, text))
        return Translation.GetLabel("ChangeChAnnounceFail");
      Logger.warning(Translation.GetLabel("ChangeChAnnounceWarn", (object) (channelId + 1), (object) (GameConfig.serverId + 1), (object) text));
      return Translation.GetLabel("ChangeChAnnounceSucc");
    }

    public static string SetAllChannelsNotice(string str)
    {
      string text = str.Substring(6);
      if (!ChannelsXml.updateNotice(text))
        return Translation.GetLabel("ChangeChsAnnounceFail");
      Logger.warning(Translation.GetLabel("ChangeChsAnnounceWarn", (object) text));
      return Translation.GetLabel("ChangeChsAnnounceSucc");
    }
  }
}
