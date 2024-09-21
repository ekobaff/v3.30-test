// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_CS_CLIENT_CLAN_CONTEXT_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using System;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_CS_CLIENT_CLAN_CONTEXT_ACK : SendPacket
  {
    private int clansCount;

    public PROTOCOL_CS_CLIENT_CLAN_CONTEXT_ACK(int count) => this.clansCount = count;

    public override void write()
    {
      this.writeH((short) 1800);
      this.writeD(this.clansCount);
      this.writeC((byte) 15);
      this.writeH((ushort) Math.Ceiling((double) this.clansCount / 15.0));
      this.writeD(uint.Parse(DateTime.Now.ToString("MMddHHmmss")));
    }
  }
}
