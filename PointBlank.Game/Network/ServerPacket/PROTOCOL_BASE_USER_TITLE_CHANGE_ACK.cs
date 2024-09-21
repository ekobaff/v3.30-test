// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BASE_USER_TITLE_CHANGE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BASE_USER_TITLE_CHANGE_ACK : SendPacket
  {
    private int _slots;
    private uint _erro;

    public PROTOCOL_BASE_USER_TITLE_CHANGE_ACK(uint erro, int slots)
    {
      this._erro = erro;
      this._slots = slots;
    }

    public override void write()
    {
      this.writeH((short) 585);
      this.writeD(this._erro);
      this.writeD(this._slots);
    }
  }
}
