// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_AUTH_SHOP_REPAIRLIST_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Managers;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_AUTH_SHOP_REPAIRLIST_ACK : SendPacket
  {
    private int Total;
    private ShopData Data;

    public PROTOCOL_AUTH_SHOP_REPAIRLIST_ACK(ShopData Data, int Total)
    {
      this.Data = Data;
      this.Total = Total;
    }

    public override void write()
    {
      this.writeH((short) 1070);
      this.writeD(this.Total);
      this.writeD(this.Data.ItemsCount);
      this.writeD(this.Data.Offset);
      this.writeB(this.Data.Buffer);
      this.writeD(585);
    }
  }
}
