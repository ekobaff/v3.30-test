// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_AUTH_ACCOUNT_KICK_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_AUTH_ACCOUNT_KICK_ACK : SendPacket
  {
    private int _type;

    public PROTOCOL_AUTH_ACCOUNT_KICK_ACK(int type) => this._type = type;

    public override void write()
    {
      this.writeH((short) 998);
      this.writeC((byte) this._type);
    }
  }
}
