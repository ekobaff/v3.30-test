// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_AUTH_SHOP_RECV_GIFT_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Account;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_AUTH_SHOP_RECV_GIFT_ACK : SendPacket
  {
    private Message gift;

    public PROTOCOL_AUTH_SHOP_RECV_GIFT_ACK(Message gift) => this.gift = gift;

    public override void write()
    {
      this.writeH((short) 1079);
      this.writeD(this.gift.object_id);
      this.writeD((uint) this.gift.sender_id);
      this.writeD(this.gift.state);
      this.writeD((uint) this.gift.expireDate);
      this.writeC((byte) (this.gift.sender_name.Length + 1));
      this.writeS(this.gift.sender_name, this.gift.sender_name.Length + 1);
      this.writeC((byte) 6);
      this.writeS("EVENT", 6);
    }
  }
}
