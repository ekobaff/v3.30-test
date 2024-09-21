// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BATTLE_GIVEUPBATTLE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BATTLE_GIVEUPBATTLE_ACK : SendPacket
  {
    private Account p;
    private int type;

    public PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(Account p, int type)
    {
      this.p = p;
      this.type = type;
    }

    public override void write()
    {
      if (this.p == null)
        return;
      this.writeH((short) 4110);
      this.writeD(this.p._slotId);
      this.writeC((byte) this.type);
      this.writeD(this.p._exp);
      this.writeD(this.p._rank);
      this.writeD(this.p._gp);
      this.writeD(this.p._statistic.escapes);
      this.writeD(this.p._statistic.escapes);
      this.writeD(0);
      this.writeC((byte) 0);
    }
  }
}
