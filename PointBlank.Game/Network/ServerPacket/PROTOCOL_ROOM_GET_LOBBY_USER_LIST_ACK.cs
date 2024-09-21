// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_ROOM_GET_LOBBY_USER_LIST_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using System;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_ROOM_GET_LOBBY_USER_LIST_ACK : SendPacket
  {
    private List<Account> players;
    private List<int> playersIdxs;

    public PROTOCOL_ROOM_GET_LOBBY_USER_LIST_ACK(Channel ch)
    {
      this.players = ch.getWaitPlayers();
      this.playersIdxs = this.GetRandomIndexes(this.players.Count, this.players.Count >= 8 ? 8 : this.players.Count);
    }

    private List<int> GetRandomIndexes(int total, int count)
    {
      if (total == 0 || count == 0)
        return new List<int>();
      Random random = new Random();
      List<int> intList = new List<int>();
      for (int index = 0; index < total; ++index)
        intList.Add(index);
      for (int index1 = 0; index1 < intList.Count; ++index1)
      {
        int index2 = random.Next(intList.Count);
        int num = intList[index1];
        intList[index1] = intList[index2];
        intList[index2] = num;
      }
      return intList.GetRange(0, count);
    }

    public override void write()
    {
      this.writeH((short) 3930);
      this.writeD(this.playersIdxs.Count);
      for (int index = 0; index < this.playersIdxs.Count; ++index)
      {
        Account player = this.players[this.playersIdxs[index]];
        this.writeD(player.getSessionId());
        this.writeD(player.getRank());
        this.writeC((byte) (player.player_name.Length + 1));
        this.writeUnicode(player.player_name, true);
        this.writeC((byte) player.name_color);
      }
    }
  }
}
