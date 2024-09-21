﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK : SendPacket
  {
    private Friend _f;
    private int _index;
    private FriendState _state;
    private FriendChangeState _type;

    public PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState type, Friend friend, int idx)
    {
      this._type = type;
      this._state = (FriendState) friend.state;
      this._f = friend;
      this._index = idx;
    }

    public PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(
      FriendChangeState type,
      Friend friend,
      int state,
      int idx)
    {
      this._type = type;
      this._state = (FriendState) state;
      this._f = friend;
      this._index = idx;
    }

    public PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(
      FriendChangeState type,
      Friend friend,
      FriendState state,
      int idx)
    {
      this._type = type;
      this._state = state;
      this._f = friend;
      this._index = idx;
    }

    public override void write()
    {
      this.writeH((short) 791);
      this.writeC((byte) this._type);
      this.writeC((byte) this._index);
      if (this._type == FriendChangeState.Insert || this._type == FriendChangeState.Update)
      {
        PlayerInfo player = this._f.player;
        if (player == null)
        {
          this.writeB(new byte[15]);
        }
        else
        {
          this.writeC((byte) (player.player_name.Length + 1));
          this.writeUnicode(player.player_name, true);
          this.writeQ(this._f.player_id);
          this.writeD(ComDiv.GetFriendStatus(this._f, this._state));
          this.writeC((byte) player._rank);
          this.writeC((byte) 0);
        }
      }
      else
        this.writeB(new byte[17]);
    }
  }
}
