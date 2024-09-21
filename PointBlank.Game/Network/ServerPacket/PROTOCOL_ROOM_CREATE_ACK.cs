// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_ROOM_CREATE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_ROOM_CREATE_ACK : SendPacket
  {
    private Account leader;
    private Room room;
    private uint erro;

    public PROTOCOL_ROOM_CREATE_ACK(uint err, Room r, Account p)
    {
      this.erro = err;
      this.room = r;
      this.leader = p;
    }

    public override void write()
    {
      this.writeH((short) 3842);
      this.writeD(this.erro == 0U ? (uint) this.room._roomId : this.erro);
      if (this.erro != 0U)
        return;
      this.writeD(this.room._roomId);
      this.writeUnicode(this.room.name, 46);
      this.writeC((byte) this.room.mapId);
      this.writeC((byte) this.room.rule);
      this.writeC(this.room.stage);
      this.writeC((byte) this.room.room_type);
      this.writeC((byte) this.room._state);
      this.writeC((byte) this.room.getAllPlayers().Count);
      this.writeC((byte) this.room.getSlotCount());
      this.writeC((byte) this.room._ping);
      this.writeH((ushort) this.room.weaponsFlag);
      this.writeD(this.room.getFlag());
      this.writeC((byte) 0);
      this.writeC((byte) 0);
      this.writeC((byte) 5);
      this.writeUnicode(this.leader.player_name, 66);
      this.writeD(this.room.killtime);
      this.writeC(this.room.Limit);
      this.writeC(this.room.WatchRuleFlag);
      this.writeH(this.room.BalanceType);
      this.writeB(new byte[16]);
    }
  }
}
