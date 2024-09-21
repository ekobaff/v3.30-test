// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_ROOM_CHECK_MAIN_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_ROOM_CHECK_MAIN_ACK : SendPacket
  {
    private uint _slot;

    public PROTOCOL_ROOM_CHECK_MAIN_ACK(uint slot) => this._slot = slot;

    public override void write()
    {
      this.writeH((short) 3882);
      this.writeD(this._slot);
    }
  }
}
