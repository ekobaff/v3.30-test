// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_AUTH_SHOP_DELETE_ITEM_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_AUTH_SHOP_DELETE_ITEM_ACK : SendPacket
  {
    private long _objId;
    private uint _erro;

    public PROTOCOL_AUTH_SHOP_DELETE_ITEM_ACK(uint erro, long objId = 0)
    {
      this._erro = erro;
      if (erro != 1U)
        return;
      this._objId = objId;
    }

    public override void write()
    {
      this.writeH((short) 1056);
      this.writeD(this._erro);
      if (this._erro != 1U)
        return;
      this.writeD((int) this._objId);
    }
  }
}
