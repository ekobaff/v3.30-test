// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_CLAN_WAR_JOIN_ROOM_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_CLAN_WAR_JOIN_ROOM_ACK : SendPacket
  {
    private Match _mt;
    private int _roomId;
    private int _team;

    public PROTOCOL_CLAN_WAR_JOIN_ROOM_ACK(Match match, int roomId, int team)
    {
      this._mt = match;
      this._roomId = roomId;
      this._team = team;
    }

    public override void write()
    {
      this.writeH((short) 1566);
      this.writeD(this._roomId);
      this.writeH((ushort) this._team);
      this.writeH((ushort) this._mt.getServerInfo());
    }
  }
}
