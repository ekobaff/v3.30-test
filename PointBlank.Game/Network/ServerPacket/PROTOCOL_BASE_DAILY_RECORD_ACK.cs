// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BASE_DAILY_RECORD_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BASE_DAILY_RECORD_ACK : SendPacket
  {
    private PlayerDailyRecord Record;

    public PROTOCOL_BASE_DAILY_RECORD_ACK(PlayerDailyRecord Record) => this.Record = Record;

    public override void write()
    {
      this.writeH((short) 623);
      this.writeH((short) this.Record.Total);
      this.writeH((short) this.Record.Wins);
      this.writeH((short) this.Record.Loses);
      this.writeH((short) this.Record.Draws);
      this.writeH((short) this.Record.Kills);
      this.writeH((short) this.Record.Headshots);
      this.writeH((short) this.Record.Deaths);
      this.writeD(this.Record.Exp);
      this.writeD(this.Record.Point);
      this.writeD((short) this.Record.Playtime); // Value 0
      this.writeC((byte) 0);
      this.writeD(0);
      this.writeH((short) 1);
      this.writeD(0);
      this.writeD(0);
      this.writeD(0);
      this.writeC((byte) 0);
    }
  }
}
