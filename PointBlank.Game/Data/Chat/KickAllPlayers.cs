// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.KickAllPlayers
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System.Collections.Generic;

namespace PointBlank.Game.Data.Chat
{
  public static class KickAllPlayers
  {
    public static string KickPlayers()
    {
      int num = 0;
      using (PROTOCOL_AUTH_ACCOUNT_KICK_ACK authAccountKickAck = new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(0))
      {
        if (GameManager._socketList.Count > 0)
        {
          byte[] completeBytes = authAccountKickAck.GetCompleteBytes("KickAllPlayers.genCode");
          foreach (GameClient gameClient in (IEnumerable<GameClient>) GameManager._socketList.Values)
          {
            Account player = gameClient._player;
            if (player != null && player._isOnline && player.access <= AccessLevel.Streamer)
            {
              player.SendCompletePacket(completeBytes);
              player.Close(1000, true);
              ++num;
            }
          }
        }
      }
      return Translation.GetLabel("KickAllWarn", (object) num);
    }
  }
}
