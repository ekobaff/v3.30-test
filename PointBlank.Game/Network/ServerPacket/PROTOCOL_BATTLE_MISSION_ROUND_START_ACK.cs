// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BATTLE_MISSION_ROUND_START_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Utils;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BATTLE_MISSION_ROUND_START_ACK : SendPacket
  {
    private Room _r;

    public PROTOCOL_BATTLE_MISSION_ROUND_START_ACK(Room r) => this._r = r;

    public override void write()
    {
      this.writeH((short) 4129);
      this.writeC((byte) this._r.rounds);
      this.writeD(this._r.getInBattleTimeLeft());
      this.writeH(AllUtils.getSlotsFlag(this._r, true, false));
      this.writeC((byte) 0);
    }
  }
}
