// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BATTLE_NOTIFY_HACK_USER_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BATTLE_NOTIFY_HACK_USER_ACK : SendPacket
  {
    private int slotId;

    public PROTOCOL_BATTLE_NOTIFY_HACK_USER_ACK(int slot) => this.slotId = slot;

    public override void write()
    {
      this.writeH((short) 3413);
      this.writeC((byte) this.slotId);
      this.writeC((byte) 1);
      this.writeD(1);
    }
  }
}
