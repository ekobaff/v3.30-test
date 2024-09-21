// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BASE_USER_TITLE_INFO_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using System;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BASE_USER_TITLE_INFO_ACK : SendPacket
  {
    private Account p;

    public PROTOCOL_BASE_USER_TITLE_INFO_ACK(Account player) => this.p = player;

    public override void write()
    {
      this.writeH((short) 591);
      this.writeB(BitConverter.GetBytes(this.p.player_id), 0, 4);
      this.writeQ(this.p._titles.Flags);
      this.writeC((byte) this.p._titles.Equiped1);
      this.writeC((byte) this.p._titles.Equiped2);
      this.writeC((byte) this.p._titles.Equiped3);
      this.writeD(this.p._titles.Slots);
    }
  }
}
