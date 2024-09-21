// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_CLAN_WAR_ENEMY_INFO_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_CLAN_WAR_ENEMY_INFO_ACK : SendPacket
  {
    public Match mt;

    public PROTOCOL_CLAN_WAR_ENEMY_INFO_ACK(Match match) => this.mt = match;

    public override void write()
    {
      this.writeH((short) 1574);
      this.writeH((short) this.mt.getServerInfo());
      this.writeC((byte) this.mt._matchId);
      this.writeC((byte) this.mt.friendId);
      this.writeC((byte) this.mt.formação);
      this.writeC((byte) this.mt.getCountPlayers());
      this.writeD(this.mt._leader);
      this.writeC((byte) 0);
      this.writeD(this.mt.clan._id);
      this.writeC((byte) this.mt.clan._rank);
      this.writeD(this.mt.clan._logo);
      this.writeS(this.mt.clan._name, 17);
      this.writeT(this.mt.clan._pontos);
      this.writeC((byte) this.mt.clan._name_color);
    }
  }
}
