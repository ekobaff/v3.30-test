// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_LOBBY_LEAVE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_LOBBY_LEAVE_ACK : SendPacket
  {
    private uint _erro;

    public PROTOCOL_LOBBY_LEAVE_ACK(uint erro) => this._erro = erro;

    public override void write()
    {
      this.writeH((short) 3076);
      this.writeD(this._erro);
    }
  }
}
