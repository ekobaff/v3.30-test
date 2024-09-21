// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BATTLE_START_COUNTDOWN_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BATTLE_START_COUNTDOWN_ACK : SendPacket
  {
    private CountDownEnum type;

    public PROTOCOL_BATTLE_START_COUNTDOWN_ACK(CountDownEnum timer) => this.type = timer;

    public override void write()
    {
      this.writeH((short) 4102);
      this.writeC((byte) this.type);
    }
  }
}
