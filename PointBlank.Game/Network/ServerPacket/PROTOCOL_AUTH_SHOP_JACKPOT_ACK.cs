// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_AUTH_SHOP_JACKPOT_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_AUTH_SHOP_JACKPOT_ACK : SendPacket
  {
    private string _w;
    private int cupomId;
    private int _random;

    public PROTOCOL_AUTH_SHOP_JACKPOT_ACK(string winner, int cupom, int rnd)
    {
      this._w = winner;
      this.cupomId = cupom;
      this._random = rnd;
    }

    public override void write()
    {
      this.writeH((short) 1068);
      this.writeH((short) 0);
      this.writeC((byte) this._random);
      this.writeD(this.cupomId);
      this.writeC((byte) this._w.Length);
      this.writeUnicode(this._w, false);
    }
  }
}
