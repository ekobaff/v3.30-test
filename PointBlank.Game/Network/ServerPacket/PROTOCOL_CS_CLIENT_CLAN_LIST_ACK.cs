// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_CS_CLIENT_CLAN_LIST_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_CS_CLIENT_CLAN_LIST_ACK : SendPacket
  {
    private uint _page;
    private int _count;
    private byte[] _array;

    public PROTOCOL_CS_CLIENT_CLAN_LIST_ACK(uint page, int count, byte[] array)
    {
      this._page = page;
      this._count = count;
      this._array = array;
    }

    public override void write()
    {
      this.writeH((short) 1798);
      this.writeD(this._page);
      this.writeC((byte) this._count);
      this.writeB(this._array);
    }
  }
}
