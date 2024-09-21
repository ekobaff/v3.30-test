// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_ACK : SendPacket
  {
    private uint _erro;

    public PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_ACK(EventErrorEnum erro) => this._erro = (uint) erro;

    public override void write()
    {
      this.writeH((short) 547);
      this.writeD(this._erro);
    }
  }
}
