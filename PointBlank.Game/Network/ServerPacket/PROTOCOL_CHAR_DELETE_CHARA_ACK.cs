// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_CHAR_DELETE_CHARA_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_CHAR_DELETE_CHARA_ACK : SendPacket
  {
    private uint Error;
    private int Slot;
    private PointBlank.Game.Data.Model.Account Player;
    private ItemsModel Item;

    public PROTOCOL_CHAR_DELETE_CHARA_ACK(uint Error, int Slot = 0, PointBlank.Game.Data.Model.Account Player = null, ItemsModel Item = null)
    {
      this.Error = Error;
      this.Slot = Slot;
      this.Player = Player;
      this.Item = Item;
    }

    public override void write()
    {
      this.writeH((short) 6152);
      this.writeD(this.Error);
      if (this.Error != 0U)
        return;
      int idStatics = ComDiv.getIdStatics(this.Item._id, 2);
      this.writeC((byte) this.Slot);
      this.writeD((int) this.Item._objId);
      if (idStatics == 1)
      {
        this.writeD(this.Player.getCharacter(this.Player._equip._red).Slot);
      }
      else
      {
        if (idStatics != 2)
          return;
        this.writeD(this.Player.getCharacter(this.Player._equip._blue).Slot);
      }
    }
  }
}
