// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_CS_ROOM_INVITED_RESULT_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_CS_ROOM_INVITED_RESULT_ACK : SendPacket
  {
    private long _pId;

    public PROTOCOL_CS_ROOM_INVITED_RESULT_ACK(long pId) => this._pId = pId;

    public override void write()
    {
      this.writeH((short) 1903);
      this.writeQ(this._pId);
    }
  }
}
