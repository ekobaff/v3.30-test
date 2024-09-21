// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_SHOP_REPAIR_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_SHOP_REPAIR_ACK : SendPacket
  {
    private uint Error;
    private ItemsModel Item;
    private PointBlank.Game.Data.Model.Account Player;

    public PROTOCOL_SHOP_REPAIR_ACK(uint Error, ItemsModel Item = null, PointBlank.Game.Data.Model.Account Player = null)
    {
      this.Error = Error;
      if (Error != 1U)
        return;
      if (Item != null)
        this.Item = Item;
      else
        Error = 2147483648U;
      this.Player = Player;
    }

    public override void write()
    {
      this.writeH((short) 1077);
      this.writeH((short) 0);
      this.writeD(this.Error);
      if (this.Error != 1U)
        return;
      this.writeC((byte) 0);
      this.writeD(this.Item._id);
      this.writeD(this.Player._money);
      this.writeD(this.Player._gp);
    }
  }
}
