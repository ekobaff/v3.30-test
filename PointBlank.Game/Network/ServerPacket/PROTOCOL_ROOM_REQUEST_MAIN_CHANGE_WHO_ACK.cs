// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK : SendPacket
  {
    private uint _slot;

    public PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK(uint slot) => this._slot = slot;

    public PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK(int slot) => this._slot = (uint) slot;

    public override void write()
    {
      this.writeH((short) 3880);
      this.writeD(this._slot);
    }
  }
}
