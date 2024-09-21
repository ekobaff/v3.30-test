// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BASE_ATTENDANCE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Managers.Events;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BASE_ATTENDANCE_ACK : SendPacket
  {
    private EventVisitModel _event;
    private PlayerEvent _pev;
    private uint _erro;
    private int day;

    public PROTOCOL_BASE_ATTENDANCE_ACK(
      EventErrorEnum erro,
      EventVisitModel ev,
      PlayerEvent pev,
      int day)
    {
      this._erro = (uint) erro;
      this._event = ev;
      this._pev = pev;
      this.day = day;
    }

    public override void write()
    {
      this.writeH((short) 545);
      this.writeD(this._erro);
      if (this._erro != 2147489028U)
        return;
      this.writeD(this._event.id);
      this.writeC((byte) (this._pev.LastVisitSequence1 + 1));
      this.writeC((byte) (this._pev.LastVisitSequence2 + 1));
      this.writeH((short) 1);
      this.writeC((byte) this.day);
    }
  }
}
