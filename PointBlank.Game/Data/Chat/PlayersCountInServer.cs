// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.PlayersCountInServer
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Servers;
using PointBlank.Core.Xml;
using PointBlank.Game.Data.Configs;

namespace PointBlank.Game.Data.Chat
{
  public static class PlayersCountInServer
  {
    public static string GetMyServerPlayersCount() => Translation.GetLabel("UsersCount", (object) GameManager._socketList.Count, (object) GameConfig.serverId);

    public static string GetServerPlayersCount(string str)
    {
      int id = int.Parse(str.Substring(9));
      GameServerModel server = ServersXml.getServer(id);
      if (server == null)
        return Translation.GetLabel("UsersInvalid");
      return Translation.GetLabel("UsersCount2", (object) server._LastCount, (object) server._maxPlayers, (object) id);
    }
  }
}
