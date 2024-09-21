// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_CS_CLAN_MATCH_RESULT_CONTEXT_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using System;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_CS_CLAN_MATCH_RESULT_CONTEXT_ACK : SendPacket
  {
    private int matchCount;

    public PROTOCOL_CS_CLAN_MATCH_RESULT_CONTEXT_ACK(int count) => this.matchCount = count;

    public override void write()
    {
      this.writeH((short) 1955);
      this.writeC((byte) this.matchCount);
      this.writeC((byte) 13);
      this.writeC((byte) Math.Ceiling((double) this.matchCount / 13.0));
    }
  }
}
