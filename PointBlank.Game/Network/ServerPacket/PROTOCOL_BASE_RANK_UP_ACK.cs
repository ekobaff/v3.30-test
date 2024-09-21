// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BASE_RANK_UP_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BASE_RANK_UP_ACK : SendPacket
  {
    private int _rank;
    private int _allExp;

    public PROTOCOL_BASE_RANK_UP_ACK(int rank, int allExp)
    {
      this._rank = rank;
      this._allExp = allExp;
    }

    public override void write()
    {
      this.writeH((short) 551);
      this.writeD(this._rank);
      this.writeD(this._rank);
      this.writeD(this._allExp);
    }
  }
}
