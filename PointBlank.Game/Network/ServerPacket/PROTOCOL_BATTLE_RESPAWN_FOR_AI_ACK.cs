// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BATTLE_RESPAWN_FOR_AI_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BATTLE_RESPAWN_FOR_AI_ACK : SendPacket
  {
    private int _slot;

    public PROTOCOL_BATTLE_RESPAWN_FOR_AI_ACK(int slot) => this._slot = slot;

    public override void write()
    {
      this.writeH((short) 4151);
      this.writeD(this._slot);
    }
  }
}
