// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_SERVER_MESSAGE_KICK_BATTLE_PLAYER_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_SERVER_MESSAGE_KICK_BATTLE_PLAYER_ACK : SendPacket
  {
    private EventErrorEnum _error;

    public PROTOCOL_SERVER_MESSAGE_KICK_BATTLE_PLAYER_ACK(EventErrorEnum error) => this._error = error;

    public override void write()
    {
      this.writeH((short) 2564);
      this.writeD((uint) this._error);
    }
  }
}
