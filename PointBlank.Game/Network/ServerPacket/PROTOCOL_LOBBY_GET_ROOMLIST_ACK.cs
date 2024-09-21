// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_LOBBY_GET_ROOMLIST_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_LOBBY_GET_ROOMLIST_ACK : SendPacket
  {
    private int RoomPage;
    private int PlayerPage;
    private int AllPlayers;
    private int AllRooms;
    private int CountRoom;
    private int CountPlayer;
    private byte[] Rooms;
    private byte[] Players;

    public PROTOCOL_LOBBY_GET_ROOMLIST_ACK(
      int AllRooms,
      int AllPlayers,
      int RoomPage,
      int PlayerPage,
      int CountRoom,
      int CountPlayer,
      byte[] Rooms,
      byte[] Players)
    {
      this.AllRooms = AllRooms;
      this.AllPlayers = AllPlayers;
      this.RoomPage = RoomPage;
      this.PlayerPage = PlayerPage;
      this.Rooms = Rooms;
      this.Players = Players;
      this.CountRoom = CountRoom;
      this.CountPlayer = CountPlayer;
    }

    public override void write()
    {
      this.writeH((short) 3078);
      this.writeD(this.AllRooms);
      this.writeD(this.RoomPage);
      this.writeD(this.CountRoom);
      this.writeB(this.Rooms);
      this.writeD(this.AllPlayers);
      this.writeD(this.PlayerPage);
      this.writeD(this.CountPlayer);
      this.writeB(this.Players);
    }
  }
}
