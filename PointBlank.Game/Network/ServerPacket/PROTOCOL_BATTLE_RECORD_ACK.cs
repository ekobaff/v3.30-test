// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BATTLE_RECORD_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BATTLE_RECORD_ACK : SendPacket
  {
    private PointBlank.Game.Data.Model.Room _r;

    public PROTOCOL_BATTLE_RECORD_ACK(PointBlank.Game.Data.Model.Room r) => this._r = r;

    public override void write()
    {
      this.writeH((short) 4139);
      this.writeH((ushort) this._r._redKills);
      this.writeH((ushort) this._r._redDeaths);
      this.writeH((ushort) this._r._redAssists);
      this.writeH((ushort) this._r._blueKills);
      this.writeH((ushort) this._r._blueDeaths);
      this.writeH((ushort) this._r._blueAssists);
      for (int index = 0; index < 16; ++index)
      {
        Slot slot = this._r._slots[index];
        this.writeH((ushort) slot.allKills);
        this.writeH((ushort) slot.allDeaths);
        this.writeH((ushort) slot.allAssists);
      }
    }
  }
}
