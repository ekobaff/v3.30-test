// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_CS_JOIN_REQUEST_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_CS_JOIN_REQUEST_ACK : SendPacket
  {
    private int _clanId;
    private uint _erro;

    public PROTOCOL_CS_JOIN_REQUEST_ACK(uint erro, int clanId)
    {
      this._erro = erro;
      this._clanId = clanId;
    }

    public override void write()
    {
      this.writeH((short) 1837);
      this.writeD(this._erro);
      if (this._erro != 0U)
        return;
      this.writeD(this._clanId);
    }
  }
}
