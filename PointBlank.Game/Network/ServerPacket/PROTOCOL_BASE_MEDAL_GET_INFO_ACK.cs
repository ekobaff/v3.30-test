// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BASE_MEDAL_GET_INFO_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BASE_MEDAL_GET_INFO_ACK : SendPacket
  {
    private Account p;

    public PROTOCOL_BASE_MEDAL_GET_INFO_ACK(Account p) => this.p = p;

    public override void write()
    {
      this.writeH((short) 571);
      if (this.p != null)
      {
        this.writeQ(this.p.player_id);
        this.writeD(this.p.brooch);
        this.writeD(this.p.insignia);
        this.writeD(this.p.medal);
        this.writeD(this.p.blue_order);
      }
      else
        this.writeB(new byte[24]);
    }
  }
}
