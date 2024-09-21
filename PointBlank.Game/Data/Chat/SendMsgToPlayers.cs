// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.SendMsgToPlayers
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Data.Chat
{
  public static class SendMsgToPlayers
  {
    public static string SendToAll(string str)
    {
      string msg = str.Substring(5);
      int num = 0;
      using (PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK messageAnnounceAck = new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(msg))
        num = GameManager.SendPacketToAllClients((SendPacket) messageAnnounceAck);
      return Translation.GetLabel("MsgAllClients", (object) num);
    }

    public static string SendToRoom(string str, Room room)
    {
      string msg = str.Substring(3);
      if (room == null)
        return Translation.GetLabel("GeneralRoomInvalid");
      using (PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK messageAnnounceAck = new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(msg))
        room.SendPacketToPlayers((SendPacket) messageAnnounceAck);
      return Translation.GetLabel("MsgRoomPlayers");
    }
  }
}
