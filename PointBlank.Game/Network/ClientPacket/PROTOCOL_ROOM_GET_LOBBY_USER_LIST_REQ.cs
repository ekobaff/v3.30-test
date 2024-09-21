// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_ROOM_GET_LOBBY_USER_LIST_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_ROOM_GET_LOBBY_USER_LIST_REQ : ReceivePacket
  {
    public PROTOCOL_ROOM_GET_LOBBY_USER_LIST_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      Account player = this._client._player;
      if (player == null)
        return;
      try
      {
        Channel channel = player.getChannel();
        if (channel == null)
          return;
        this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_GET_LOBBY_USER_LIST_ACK(channel));
      }
      catch (Exception ex)
      {
        Logger.warning("PROTOCOL_ROOM_GET_LOBBY_USER_LIST_REQ: " + ex.ToString());
      }
    }
  }
}
