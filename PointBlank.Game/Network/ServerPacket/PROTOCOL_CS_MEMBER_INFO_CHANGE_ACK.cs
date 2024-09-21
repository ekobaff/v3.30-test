// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK : SendPacket
  {
    private Account p;
    private ulong status;

    public PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK(Account player)
    {
      this.p = player;
      this.status = ComDiv.GetClanStatus(player._status, player._isOnline);
    }

    public PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK(Account player, FriendState st)
    {
      this.p = player;
      if (st == FriendState.None)
        this.status = ComDiv.GetClanStatus(player._status, player._isOnline);
      else
        this.status = ComDiv.GetClanStatus(st);
    }

    public override void write()
    {
      this.writeH((short) 1875);
      this.writeQ(this.p.player_id);
      this.writeQ(this.status);
    }
  }
}
