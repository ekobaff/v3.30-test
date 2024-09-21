// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.NickHistory
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System.Collections.Generic;

namespace PointBlank.Game.Data.Chat
{
  public static class NickHistory
  {
    public static string GetHistoryById(string str, Account player)
    {
      List<NHistoryModel> history = NickHistoryManager.getHistory((object) long.Parse(str.Substring(7)), 1);
      string msg = Translation.GetLabel("NickHistory1_Title");
      foreach (NHistoryModel nhistoryModel in history)
        msg = msg + "\n" + Translation.GetLabel("NickHistory1_Item", (object) nhistoryModel.from_nick, (object) nhistoryModel.to_nick, (object) nhistoryModel.date, (object) nhistoryModel.motive);
      player.SendPacket((SendPacket) new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(msg));
      return Translation.GetLabel("NickHistory1_Result", (object) history.Count);
    }

    public static string GetHistoryByNewNick(string str, Account player)
    {
      List<NHistoryModel> history = NickHistoryManager.getHistory((object) str.Substring(7), 0);
      string msg = Translation.GetLabel("NickHistory2_Title");
      foreach (NHistoryModel nhistoryModel in history)
        msg = msg + "\n" + Translation.GetLabel("NickHistory2_Item", (object) nhistoryModel.from_nick, (object) nhistoryModel.to_nick, (object) nhistoryModel.player_id, (object) nhistoryModel.date, (object) nhistoryModel.motive);
      player.SendPacket((SendPacket) new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(msg));
      return Translation.GetLabel("NickHistory2_Result", (object) history.Count);
    }
  }
}
