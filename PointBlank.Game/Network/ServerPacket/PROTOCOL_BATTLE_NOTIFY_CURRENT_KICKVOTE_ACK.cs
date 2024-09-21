// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_ACK : SendPacket
  {
    private VoteKick _k;

    public PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_ACK(VoteKick vote) => this._k = vote;

    public override void write()
    {
      this.writeH((short) 3407);
      this.writeC((byte) this._k.kikar);
      this.writeC((byte) this._k.deixar);
    }
  }
}
